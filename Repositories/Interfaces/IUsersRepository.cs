using Models.DTOs.User;

namespace Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<UserDTO>> GetUsers();
        Task<List<UserDTO>> GetActiveUsers();
        Task<List<UserDTO>> GetAllAgent();
        Task<List<UserDTO>> GetUsersWithFullName();
        Task<int> GetRoleById(int id);
        Task<UserDTO> GetUserById(int id);
        Task<List<UserDTO>> GetUsersByDistrictId(int districtId);
        Task<AgentDTO> GetAgentsWithDistrict(int agentId);
        Task<UserWithServicesDTO> SubscribeUserToService(int userId, int serviceId);
        Task<UserWithServicesDTO> GetUserWithServices(int userId);
        Task<UserDTO> PostUser(UserCreationDTO userCreationDTO, int userRole);
        Task<UserDTO> UpdateUser(UserUpdateDTO userUpdateDTO);
        Task<UserDTO> DeleteUser(int id);
    }
}