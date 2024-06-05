namespace BiteStation.Domain.Models;
public class OrderItem
{
    public int Quantity { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
}
    