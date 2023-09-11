using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.DTO.Admin.Ticket
{
    public class TicketDto
    {
        public string Name { get; set; } 
        public double Price { get; set; }
        public bool Active { get; set; }
    }
}
