using System.ComponentModel.DataAnnotations;

namespace BiteStation.Domain.Dtos;
public class ResetPasswordDto
{
    [EmailAddress]
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Token { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string? NewPassword { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string? ConfirmPassword { get; set; }
}
