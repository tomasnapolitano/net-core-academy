using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Login;
using Models.DTOs.User;
using Models.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utils.CustomValidator;
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

        public UserWithTokenDTO Login(UserLoginDTO userLoginDTO)
        {
            UserWithTokenDTO userWithToken = _usersRepository.Login(userLoginDTO).Result;
            var Token = GetToken(userWithToken);
            userWithToken.Token = Token;
            return userWithToken;
        }

        public List<UserDTO> GetUsers()
        {
            return _usersRepository.GetUsers().Result;
        }

        public List<UserDTO> GetActiveUsers()
        {
            return _usersRepository.GetActiveUsers().Result;
        }

        public List<UserDTO> GetAllAgents()
        {
            return _usersRepository.GetAllAgent().Result;
        }

        public UserDTO GetUserById(int id)
        {
            return _usersRepository.GetUserById(id).Result;
        }

        public List<UserDTO> GetUsersByDistrictId(int districtId)
        {
            return _usersRepository.GetUsersByDistrictId(districtId).Result;
        }

        public AgentDTO GetAgentsWithDistrict(int agentId)
        {
            return _usersRepository.GetAgentsWithDistrict(agentId).Result;
        }

        public UserWithServicesDTO SubscribeUserToService(int userId, int serviceId)
        {
            return _usersRepository.SubscribeUserToService(userId, serviceId).Result;
        }

        public UserWithServicesDTO PauseSubscribeUserToService(int subscriptionId)
        {
            return _usersRepository.PauseSubscribeUserToService(subscriptionId).Result;
        }

        public UserWithServicesDTO GetUserWithServices(int userId)
        {
            return _usersRepository.GetUserWithServices(userId).Result;
        }

        public UserDTO PostUser(int userId, UserCreationDTO userCreationDTO)
        {
            CustomValidatorInput<UserCreationDTO>.DTOValidator(userCreationDTO);
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

        public UserDTO UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            CustomValidatorInput<UserUpdateDTO>.DTOValidator(userUpdateDTO);
            return _usersRepository.UpdateUser(userUpdateDTO).Result;
        }

        public UserDTO DeleteUser( int adminId , int id)
        {
            var userRole = _usersRepository.GetRoleById(adminId).Result;

            if (userRole == (int)UserRoleEnum.Admin)
            {
                return _usersRepository.DeleteUser(id).Result;
            }
            else
            {
                throw new BadRequestException("El id usuario enviado por parametro no puede eliminar usuarios. No es ADMIN");
            }
        }

        public string GetToken(UserWithTokenDTO userWithTokenDTO)
        {
            var jwt = new Jwt
            {
                Key = Environment.GetEnvironmentVariable("JWT_KEY"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                Subject = Environment.GetEnvironmentVariable("JWT_SUBJECT")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userWithTokenDTO.UserId.ToString()),
                    new Claim(ClaimTypes.Role, userWithTokenDTO.RoleId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30), // token expira en 30 minutos
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwt.Issuer,
                Audience = jwt.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}