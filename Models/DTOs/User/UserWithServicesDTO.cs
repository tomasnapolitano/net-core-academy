using Models.DTOs.District;
using Models.DTOs.Service;

namespace Models.DTOs.User
{
    public class UserWithServicesDTO : UserDTO
    {
        public IList<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
    }
}
