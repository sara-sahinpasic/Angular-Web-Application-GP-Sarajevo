namespace Presentation.DTO.Admin.Vehicles
{
    public class VehicleDto
    {
        public int Number { get; set; }
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        public Guid ManufacturerId { get; set; }
        public Guid VehicleTypeId { get; set; }
        public int BuildYear { get; set; }
        public Guid VehicleId { get; set; }
        public Guid Id { get; set; }


    }
}
