using Domain.Abstractions.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public sealed class Status : Entity
    {
        public string Name { get; set; }
        public int Discount { get; set; }
    }
}
