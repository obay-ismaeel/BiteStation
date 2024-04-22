using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Domain.Models;
public class Menu : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public List<Item> Items { get; set; }
}
