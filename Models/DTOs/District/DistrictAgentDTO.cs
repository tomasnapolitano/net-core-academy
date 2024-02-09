using Models.DTOs.User;

namespace Models.DTOs.District
{
    public class DistrictAgentDTO : DistrictDTO
    {
        public UserDTO? Agent { get; set; }
    }
}