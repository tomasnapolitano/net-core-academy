using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        UserWithTokenDTO Login(UserLoginDTO userLoginDTO);
        List<UserDTO> GetUsers();
        List<UserDTO> GetActiveUsers();
        List<UserDTO> GetAllAgents();
        UserDTO GetUserById(int id);
        AgentDTO GetAgentsWithDistrict(int agentId);
        List<UserDTO> GetUsersByDistrictId(int districtId);
        UserWithServicesDTO SubscribeUserToService(int userId, int serviceId);
        UserWithServicesDTO PauseSubscribeUserToService(int subscriptionId);
        UserWithServicesDTO GetUserWithServices(int userId);
        ConsumptionDTO GetRandomSubscriptionConsumption(int subscriptionId);
        UserDTO PostUser(int userId , UserCreationDTO userCreationDTO, string token);
        UserDTO UpdateUser(UserUpdateDTO userUpdateDTO, string token);
        UserDTO DeleteUser(int id, string token);
    }
}