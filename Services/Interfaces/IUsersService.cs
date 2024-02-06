using Models.DTOs;
using Models.Entities;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        List<User> GetUsers();
        UserDTO PostUser(int userId , UserCreationDTO userCreationDTO);
    }
}