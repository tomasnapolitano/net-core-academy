namespace Models.DTOs.User
{
    public class UserCreationResponseDTO
    {
        public UserDTO User { get; set; }
        public string ActivationToken { get; set; }
    }
}
