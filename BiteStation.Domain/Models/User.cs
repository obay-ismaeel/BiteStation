using Microsoft.AspNetCore.Identity;

namespace BiteStation.Domain.Models;
public class User : IdentityUser
{
    public ICollection<Order> Orders { get; set; } = [];
    public Cart? Cart { get; set; }
}
