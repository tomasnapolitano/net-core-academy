using Models.DTOs.District;
using Models.DTOs.Service;

namespace Models.DTOs.User
{
    public class UserWithServicesDTO
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int AddressId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DniNumber { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string FullName { get; set; } = null!;
        public string Active { get; set; } = null!;
        public IList<ServiceSubscriptionDTO> ServiceSubscriptions { get; set; } = new List<ServiceSubscriptionDTO>();
    }
}
