using System.ComponentModel.DataAnnotations;

namespace BiteStation.Domain.Dtos;
public class RegisterRequestDto
{
    [EmailAddress]
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}
