using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Domain.Dtos;
public class ForgetPasswordDto
{
    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string? Email { get; set; }
}
