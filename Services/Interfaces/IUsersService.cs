using Models.Entities;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        List<User> GetUsers();
    }
}