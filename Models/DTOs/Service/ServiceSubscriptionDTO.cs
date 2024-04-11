namespace Models.DTOs.Service
{
    public class ServiceSubscriptionDTO
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public DistrictXserviceDTO DistrictXservice { get; set; }
        public DateTime StartDate { get; set; }
        public bool PauseSubscription { get; set; }
        public ServiceDTO Service { get; set; }
    }
}
