namespace Models.DTOs.Service
{
    public class DistrictXserviceDTO
    {
        public int DistrictXserviceId { get; set; }
        public int? DistrictId { get; set; }
        public int? ServiceId { get; set; }
        public bool Active { get; set; }
    }
}
