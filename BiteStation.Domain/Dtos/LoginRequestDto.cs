using System.ComponentModel.DataAnnotations;

namespace BiteStation.Domain.Dtos;
public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string? Email { get; set; }
    [Required]
    [StringLength(50)]
    public string? Password { get; set; }
}
