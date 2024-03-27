using Models.DTOs.Service;

namespace Models.DTOs.User
{
    public class UserWithServicesDTO : UserDTO
    {
        public IList<ServiceSubscriptionDTO> ServiceSubscriptions { get; set; } = new List<ServiceSubscriptionDTO>();
    }
}