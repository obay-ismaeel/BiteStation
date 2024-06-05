using BiteStation.Domain.Enums;

namespace BiteStation.Domain.Models;
public class Order : BaseEntity
{
    public OrderStatus Status { get; set; }
    public Restaurant Restaurant { get; set; }
    public int RestaurantId { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
    public ICollection<Item> Items { get; set;} = [];
    public ICollection<OrderItem> OrderItems { get; set;} = [];
    public Cart Cart { get; set; }
}
