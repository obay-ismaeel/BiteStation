using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Domain.Models;
public class Restaurant : BaseEntity
{
    //add an image later
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public ICollection<Menu> Menus { get; set; }
}
