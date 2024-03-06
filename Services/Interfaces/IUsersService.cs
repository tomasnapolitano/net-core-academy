using Models.DTOs.Login;
using Models.DTOs.User;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        UserWithTokenDTO Login(UserLoginDTO userLoginDTO);
        List<UserDTO> GetUsers();
        List<UserDTO> GetActiveUsers();
        List<UserDTO> GetAllAgents();
        List<UserDTO> GetUsersWithFullName();
        UserDTO GetUserById(int id);
        AgentDTO GetAgentsWithDistrict(int agentId);
        List<UserDTO> GetUsersByDistrictId(int districtId);
        UserWithServicesDTO SubscribeUserToService(int userId, int serviceId);
        UserWithServicesDTO PauseSubscribeUserToService(int subscriptionId);
        UserWithServicesDTO GetUserWithServices(int userId);
        UserDTO PostUser(int userId , UserCreationDTO userCreationDTO);
        UserDTO UpdateUser(UserUpdateDTO userUpdateDTO);
        UserDTO DeleteUser(int adminId , int id);
    }
}