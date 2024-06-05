namespace BiteStation.Domain.Models;
public class Cart : BaseEntity
{
    public string UserId { get; set; }
    public User User { get; set; }
    public int OrderId { get; set;}
    public Order Order { get; set;}
}
