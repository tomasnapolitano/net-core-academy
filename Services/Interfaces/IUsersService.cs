using Models.DTOs.User;

namespace Services.Interfaces
{
    public interface IUsersService
    { 
        UserDTO PostUser(int userId , UserCreationDTO userCreationDTO);
        List<UserDTO> GetUsers();
    }
}