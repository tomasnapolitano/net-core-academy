using Models.DTOs.User;

namespace Models.DTOs.Login
{
    public class UserWithTokenDTO : UserDTO
    {
        public string Token { get; set; }
    }
}
