using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class UserUpdateRequestDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public Guid Id { get; set; }
    }
}
