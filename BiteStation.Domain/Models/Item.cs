using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Domain.Models;
public class Item : BaseEntity
{
    //add an image later
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MenuId { get; set; }
    public Menu Menu { get; set; }
}
