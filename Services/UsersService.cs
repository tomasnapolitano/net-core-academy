using Models.DTOs.User;
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

        public List<UserDTO> GetUsers()
        {
            return _usersRepository.GetUsers().Result;
        }
    }
}