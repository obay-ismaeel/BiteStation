using BiteStation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Domain.Models;
public class Order : BaseEntity
{
    public OrderStatus Status { get; set; }
    public Restaurant Restaurant { get; set; }
    public int RestaurantId { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public ICollection<Item> Items { get; set;} = new List<Item>();
    public ICollection<OrderItem> OrderItems { get; set;} = new List<OrderItem>();
}
