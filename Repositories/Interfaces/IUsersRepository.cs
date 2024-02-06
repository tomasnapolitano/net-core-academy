using Models.DTOs.User;
using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<UserDTO>> GetUsers();
    }
}