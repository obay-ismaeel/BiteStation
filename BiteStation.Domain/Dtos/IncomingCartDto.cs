using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Domain.Dtos;
public class IncomingCartDto
{
    public List<CartItem> Items { get; set; }
}
