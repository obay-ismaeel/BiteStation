using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Settings;
using BiteStation.Domain.Dtos;
using BiteStation.Domain.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using BiteStation.Domain.Models;

namespace BiteStation.Infrastructure.Services;
public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly JwtOptions _jwtOptions;

    public UserService(UserManager<User> userManager, IConfiguration configuration, IEmailService emailService, IOptionsMonitor<JwtOptions> options)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
        _jwtOptions = options.CurrentValue;
    }

    public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return new UserManagerResponse { IsSuccess = false, Message = "User not found" };

        var decodedToken = Base64UrlEncoder.DecodeBytes(token);

        var stringToken = Encoding.UTF8.GetString(decodedToken);

        var result = await _userManager.ConfirmEmailAsync(user, stringToken);

        if (!result.Succeeded)
        {
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Email wasn't confirmed",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        return new UserManagerResponse
        {
            IsSuccess = true,
            Message = "Email was confirmed successfully"
        };
    }

    public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return new UserManagerResponse { Message = "No such user", IsSuccess = false };

        if (model.NewPassword != model.ConfirmPassword)
            return new UserManagerResponse { Message = "Passwords don't match" };

        var decodedTokenBytes = Base64UrlEncoder.DecodeBytes(model.Token);
        var originalToken = Encoding.UTF8.GetString(decodedTokenBytes);

        var result = await _userManager.ResetPasswordAsync(user, originalToken, model.NewPassword);

        if (!result.Succeeded)
            return new UserManagerResponse
            {
                Message = "Failed process",
                IsSuccess = false,
                Errors = result.Errors.Select(x => x.Description)
            };

        return new UserManagerResponse
        {
            IsSuccess = true,
            Message = "Reset password succeeded"
        };
    }

    public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return new UserManagerResponse { IsSuccess = false, Message = "No account is associated with this email" };

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var tokenAsBytes = Encoding.UTF8.GetBytes(token);
        var validToken = Base64UrlEncoder.Encode(tokenAsBytes);

        var url = $"{_configuration["AppURL"]}/reset-password?email={email}&token={validToken}";

        await _emailService.SendEmailAsync(email, $"Reset password",
            $"<h2>follow the link to reset the password of your account:</h2></br><a href='{url}'>click here</a>");

        return new UserManagerResponse
        {
            IsSuccess = true,
            Message = "Reset password email was sent successfully"
        };
    }

    public async Task<UserManagerResponse> LoginAsync(LoginRequestDto model)
    {
        if (model is null)
            throw new ArgumentNullException("Register model is null");

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
            return new UserManagerResponse() { IsSuccess = false, Message = "No such email" };

        var result = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!result)
            return new UserManagerResponse() { IsSuccess = false, Message = "invalid password" };

        var claims = new[]
        {
            new Claim("Email", model.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return new UserManagerResponse { IsSuccess = true, Message = tokenAsString, ExpiryDate = token.ValidTo };
    }

    public async Task<UserManagerResponse> RegisterAsync(RegisterRequestDto model)
    {
        if (model is null)
            throw new ArgumentNullException("Register model is null");

        if (model.Password != model.ConfirmPassword)
        {
            return new UserManagerResponse { IsSuccess = false, Message = "Confirm Password doesn't match the password" };
        }

        var user = new User
        {
            Email = model.Email,
            UserName = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Something went wrong",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        // send verification email

        var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);

        var validEmailToken = Base64UrlEncoder.Encode(encodedEmailToken);

        var url = $"{_configuration["AppURL"]}/api/auth/confirm-email?userid={user.Id}&token={validEmailToken}";

        await _emailService.SendEmailAsync(user.Email,
            $"Email Verification", $"<h3>navigate to the link below to verify your email address:</h3></br><a href='{url}'>click here</a>");

        // end email sending section

        return new UserManagerResponse
        {
            IsSuccess = true,
            Message = "User Created Successfully"
        };
    }

       
}
