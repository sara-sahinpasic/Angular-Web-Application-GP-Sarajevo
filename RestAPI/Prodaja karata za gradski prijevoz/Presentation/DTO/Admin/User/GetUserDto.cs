namespace Presentation.DTO.Admin.User
{
    public class GetUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool HasPendingRequest { get; set; }
    }
}
