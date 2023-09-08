using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Admin.User
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid RoleId { get; set; }
        public Guid Id { get; set; }
    }
}
