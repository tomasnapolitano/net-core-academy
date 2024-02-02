using Models.Entities;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public List<User> GetUsers()
        {
            return _usersRepository.GetUsers().Result;
        }
    }
}