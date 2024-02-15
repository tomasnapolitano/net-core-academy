using Models.DTOs.User;

namespace Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserDTO> PostUser (UserCreationDTO userCreationDTO , int userRole);
        Task<int> GetRoleById(int id);
        Task<UserDTO> GetUserById(int id);
        Task<List<UserDTO>> GetUsersByDistrictId(int districtId);
        Task<AgentDTO> GetAgentsWithDistrict(int agentId);
        Task<List<UserDTO>> GetUsers();
        Task<List<UserDTO>> GetActiveUsers();
        Task<List<UserDTO>> GetAllAgent();
        Task<List<UserDTO>> GetUsersWithFullName();
        Task<UserDTO> UpdateUser(UserUpdateDTO userUpdateDTO);
    }
}