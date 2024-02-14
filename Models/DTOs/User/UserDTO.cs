using Models.DTOs.District;

namespace Models.DTOs.User
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int AdressId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DniNumber { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string FullName { get; set; } = null!;
    }
}
