using Models.DTOs.District;
using System.Xml.Linq;

namespace Models.DTOs.User
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int AddressId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DniNumber { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string Active { get; set; } = null!;
        public DistrictDTO District { get; set; } = new DistrictDTO() { DistrictId = 0, DistrictName = "Sin Distrito"};

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}