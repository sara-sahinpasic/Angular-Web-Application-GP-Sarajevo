namespace Presentation.DTO.Driver
{
    public class MalfunctionDto
    {
        public string Description { get; set; }
        public DateTime DateOfMalufunction { get; set; }
        public bool Fixed { get; set; }
        public Guid VehicleId { get; set; }
    }
}
