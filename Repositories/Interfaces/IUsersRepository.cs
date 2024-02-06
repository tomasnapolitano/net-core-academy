using Models.DTOs;
using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetUsers();
        Task<UserDTO> PostUser (UserCreationDTO userCreationDTO , int userRole);
        Task<int> GetRoleById(int id);
    }
}