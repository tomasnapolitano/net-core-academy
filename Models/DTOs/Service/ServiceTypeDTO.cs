namespace Models.DTOs.Service
{
    public class ServiceTypeDTO
    {
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}