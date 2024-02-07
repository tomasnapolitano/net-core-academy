using Models.DTOs.User;
using Repositories.Interfaces;
using Services.Interfaces;
using Utils.Enum;
using Utils.Middleware;

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

        public UserDTO PostUser(int userId, UserCreationDTO userCreationDTO)
        {
            var userRole = _usersRepository.GetRoleById(userId).Result;

            // Si es admin crea un AGENTE o CLIENTE o ADMIN
            if (userRole == (int)UserRoleEnum.Admin)
            {
                return _usersRepository.PostUser(userCreationDTO, userCreationDTO.RoleId).Result;
            }
            // Si es agente se fuerza a que cree solamente un CLIENTE
            else if (userRole == (int)UserRoleEnum.Agent)
            {
                return _usersRepository.PostUser(userCreationDTO, (int)UserRoleEnum.Client).Result;
            }
            else
            {
                throw new BadRequestException("El id usuario enviado por parametro no puede crear usuarios. No es ADMIN/AGENTE");
            }

        }
    }
}