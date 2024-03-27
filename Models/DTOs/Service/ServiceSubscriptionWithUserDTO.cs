using Models.DTOs.User;
using Models.Entities;

namespace Models.DTOs.Service
{
    public class ServiceSubscriptionWithUserDTO
    {
        public int SubscriptionId { get; set; }
        public UserDTO User { get; set; }
        public DistrictXserviceDTO DistrictXservice { get; set; }
        public DateTime StartDate { get; set; }
        public bool PauseSubscription { get; set; }
        public ServiceDTO Service { get; set; }
    }
}
