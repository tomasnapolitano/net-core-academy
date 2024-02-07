using Models.DTOs.User;

namespace Services.Interfaces
{
    public interface IUsersService
    { 
        UserDTO GetUserById(int id);
        List<UserDTO> GetUsers();
        List<UserDTO> GetUsersWithFullName();
        UserDTO PostUser(int userId , UserCreationDTO userCreationDTO);
    }
}