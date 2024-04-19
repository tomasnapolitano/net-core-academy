using Models.DTOs.Service;
using Models.DTOs.User;

namespace Models.DTOs.District
{
    public class DistrictInfoDTO : DistrictDTO
    {
        public UserDTO? Agent { get; set; }
        public IList<ServiceDTO> Services { get; set; }
    }
}
