using BiteStation.Domain.Responses;
using BiteStation.Domain.Dtos;

namespace BiteStation.Domain.Abstractions;
public interface IUserService
{
    Task<UserManagerResponse> RegisterAsync(RegisterRequestDto model);
    Task<UserManagerResponse> LoginAsync(LoginRequestDto model);
    Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);
    Task<UserManagerResponse> ForgetPasswordAsync(string email);
    Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordDto model);
}
