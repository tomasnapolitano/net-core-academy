using Models.DTOs.District;

namespace Models.DTOs.User
{
    public class AgentDTO : UserDTO
    {
        public List<DistrictDTO> Districts { get; set; } = new List<DistrictDTO>();
    }
}