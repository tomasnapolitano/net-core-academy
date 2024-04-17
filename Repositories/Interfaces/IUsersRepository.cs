using Models.DTOs.Bill;
using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;

namespace Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserWithTokenDTO> Login(UserLoginDTO userLoginDTO);
        Task<bool> ChangePassword(UserUpdatePasswordDTO userUpdatePassDTO);
        Task<List<UserDTO>> GetUsers();
        Task<List<UserDTO>> GetActiveUsers();
        Task<List<UserDTO>> GetAllAgent();
        Task<int> GetRoleById(int id);
        Task<UserDTO> GetUserById(int id);
        Task<List<UserDTO>> GetUsersByDistrictId(int districtId);
        Task<AgentDTO> GetAgentsWithDistrict(int agentId);
        Task<UserWithServicesDTO> SubscribeUserToService(int userId, int serviceId);
        Task<UserWithServicesDTO> PauseSubscribeUserToService(int subscriptionId);
        Task<List<UserWithServicesDTO>> GetUsersWithServices();
        Task<UserWithServicesDTO> GetUserWithServicesById(int userId);
        Task<ServiceSubscriptionWithUserDTO> GetSubscription(int subscriptionId);
        Task<ConsumptionDTO> GetRandomSubscriptionConsumption(int subscriptionId);
        Task<UserDTO> PostUser(UserCreationDTO userCreationDTO, int userRole, string token);
        Task<UserDTO> UpdateUser(UserUpdateDTO userUpdateDTO, string token);
        Task<UserDTO> DeleteUser(int id, string token);
        Task<ConsumptionBillDTO> GenerateBill(int userId);
        Task<ConsumptionBillDTO> GetBillById(int billId);
        Task<List<ConsumptionBillDTO>> GetBillsByUserId(int userId);
        Task<List<ConsumptionBillDTO>> GetAllBills();
        Task<Dictionary<int, int>> GetUsersCountByDistrict();
        Task<List<UserDTO>> GetUsersWithoutBillReport();
        Task<Dictionary<string, int>> GetUsersByRoleReport();
    }
}