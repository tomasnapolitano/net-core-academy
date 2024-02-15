using Models.DTOs.User;

namespace Services.Interfaces
{
    public interface IUsersService
    { 
        List<UserDTO> GetUsers();
        List<UserDTO> GetActiveUsers();
        List<UserDTO> GetAllAgents();
        List<UserDTO> GetUsersWithFullName();
        UserDTO GetUserById(int id);
        AgentDTO GetAgentsWithDistrict(int agentId);
        List<UserDTO> GetUsersByDistrictId(int districtId);
        UserDTO PostUser(int userId , UserCreationDTO userCreationDTO);
        UserDTO UpdateUser(UserUpdateDTO userUpdateDTO);
        UserDTO DeleteUser(int adminId , int id);
    }
}