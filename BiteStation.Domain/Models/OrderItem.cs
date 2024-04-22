using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Domain.Models;
public class OrderItem
{
    public int Quantity { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
}
    