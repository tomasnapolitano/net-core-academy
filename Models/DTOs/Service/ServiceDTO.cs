namespace Models.DTOs.Service
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceName { get; set; } = null!;
        public double PricePerUnit { get; set; }
    }
}
