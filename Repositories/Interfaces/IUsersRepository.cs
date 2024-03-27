using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;

namespace Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserWithTokenDTO> Login(UserLoginDTO userLoginDTO);
        Task<List<UserDTO>> GetUsers();
        Task<List<UserDTO>> GetActiveUsers();
        Task<List<UserDTO>> GetAllAgent();
        Task<int> GetRoleById(int id);
        Task<UserDTO> GetUserById(int id);
        Task<List<UserDTO>> GetUsersByDistrictId(int districtId);
        Task<AgentDTO> GetAgentsWithDistrict(int agentId);
        Task<UserWithServicesDTO> SubscribeUserToService(int userId, int serviceId);
        Task<UserWithServicesDTO> PauseSubscribeUserToService(int subscriptionId);
        Task<UserWithServicesDTO> GetUserWithServices(int userId);
        Task<ServiceSubscriptionWithUserDTO> GetSubscription(int subscriptionId);
        Task<UserDTO> PostUser(UserCreationDTO userCreationDTO, int userRole, string token);
        Task<UserDTO> UpdateUser(UserUpdateDTO userUpdateDTO, string token);
        Task<UserDTO> DeleteUser(int id, string token);
    }
}