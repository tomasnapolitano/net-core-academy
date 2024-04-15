using Models.DTOs.Bill;
using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        UserWithTokenDTO Login(UserLoginDTO userLoginDTO);
        bool ChangePassword(UserUpdatePasswordDTO userUpdatePassDTO);
        List<UserDTO> GetUsers();
        List<UserDTO> GetActiveUsers();
        List<UserDTO> GetAllAgents();
        UserDTO GetUserById(int id);
        AgentDTO GetAgentsWithDistrict(int agentId);
        List<UserDTO> GetUsersByDistrictId(int districtId);
        UserWithServicesDTO SubscribeUserToService(int userId, int serviceId);
        UserWithServicesDTO PauseSubscribeUserToService(int subscriptionId);
        UserWithServicesDTO GetUserWithServicesById(int userId);
        List<UserWithServicesDTO> GetUsersWithServices();
        ServiceSubscriptionWithUserDTO GetServiceSubscriptionClient(int subscriptionId);
        ConsumptionDTO GetRandomSubscriptionConsumption(int subscriptionId);
        ConsumptionBillDTO GenerateBill(int userId);
        ConsumptionBillDTO GetBillById(int billId);
        List<ConsumptionBillDTO> GetBillsByUserId(int userId);
        List<ConsumptionBillDTO> GetAllBills();
        UserDTO PostUser(UserCreationDTO userCreationDTO, string token);
        UserDTO UpdateUser(UserUpdateDTO userUpdateDTO, string token);
        UserDTO DeleteUser(int id, string token);
    }
}