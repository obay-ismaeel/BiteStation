using AutoMapper;
using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Dtos;
using BiteStation.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiteStation.Presentation.Controllers;

public class AuthController : BaseController
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    public AuthController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService, IUserService userService, IEmailService emailService) : base(unitOfWork, mapper, imageService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto model)
    {
        var result = await _userService.LoginAsync(model);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        await _emailService.SendEmailAsync(model.Email!, "Successful Login", "Welcome back user");

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto model)
    {
        var result = await _userService.RegisterAsync(model);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userid, string token)
    {
        if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token))
            return NotFound();

        var result = await _userService.ConfirmEmailAsync(userid, token);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model)
    {
        if (string.IsNullOrWhiteSpace(model.Email))
            return BadRequest();

        var result = await _userService.ForgetPasswordAsync(model.Email);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
    {
        var result = await _userService.ResetPasswordAsync(model);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
    