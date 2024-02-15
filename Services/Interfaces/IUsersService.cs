using Models.DTOs.User;

namespace Services.Interfaces
{
    public interface IUsersService
    { 
        UserDTO GetUserById(int id);
        AgentDTO GetAgentsWithDistrict(int agentId);
        List<UserDTO> GetUsers();
        List<UserDTO> GetUsersByDistrictId(int districtId);
        List<UserDTO> GetAllAgents();
        List<UserDTO> GetUsersWithFullName();
        UserDTO PostUser(int userId , UserCreationDTO userCreationDTO);
        UserDTO UpdateUser(UserUpdateDTO userUpdateDTO);
        UserDTO DeleteUser(int adminId , int id);
    }
}