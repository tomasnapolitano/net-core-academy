using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetUsers();
    }
}