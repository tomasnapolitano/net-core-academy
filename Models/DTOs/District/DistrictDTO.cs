using Models.DTOs.User;

namespace Models.DTOs.District
{
    public class DistrictDTO
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; } = null!;
        public UserDTO? Agent { get; set; }
    }
}