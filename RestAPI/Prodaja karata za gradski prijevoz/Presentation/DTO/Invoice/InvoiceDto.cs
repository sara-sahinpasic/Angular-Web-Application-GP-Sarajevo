using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.DTO.Invoice
{
    public sealed class InvoiceDto
    {
        public string TicketName { get; set; }
        //public Guid RelationId { get; set; } ToDo: add later
        public double Price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
