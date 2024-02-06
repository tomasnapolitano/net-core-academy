using Models.DTOs.User;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        List<UserDTO> GetUsers();
    }
}