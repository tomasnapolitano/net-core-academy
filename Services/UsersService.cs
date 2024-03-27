using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Login;
using Models.DTOs.Service;
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

        public ConsumptionDTO GetRandomSubscriptionConsumption(int subscriptionId)
        {
            var subscription = _usersRepository.GetSubscription(subscriptionId).Result;

            if (subscription.PauseSubscription == true)
                throw new UnavailableServiceException("La suscripción está pausada actualmente.");

            if (subscription.DistrictXservice.Active == false)
                throw new UnavailableServiceException("El servicio no se encuentra disponible para este distrito actualmente.");

            if (subscription.Service.Active == false)
                throw new UnavailableServiceException("El servicio no se encuentra disponible en este momento.");

            // Se calcula la cantidad de días a cobrar:
            DateTime firstDayCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime firstDate = subscription.StartDate < firstDayCurrentMonth ? firstDayCurrentMonth : subscription.StartDate;
            TimeSpan daysOfConsumptionSpan = DateTime.Today - firstDate;
            int daysOfConsumption = (int)daysOfConsumptionSpan.TotalDays;

            // Se genera un número random de consumo (igual para todos los días):
            Random random = new Random();
            float dailyConsumption = (float)(random.NextDouble() * 10);

            // Calculo el costo total de consumo:
            float totalConsumption = daysOfConsumption * dailyConsumption;
            ConsumptionDTO consumptionDTO = new ConsumptionDTO()
            {
                DaysOfConsumption = daysOfConsumption,
                UnitsConsumed = totalConsumption,
                TotalCost = (float)(totalConsumption * subscription.Service.PricePerUnit),
                ServiceSubscription = subscription
            };

            return consumptionDTO;
        }

        public UserDTO PostUser(int userId, UserCreationDTO userCreationDTO, string token)
        {
            CustomValidatorInput<UserCreationDTO>.DTOValidator(userCreationDTO);

            return _usersRepository.PostUser(userCreationDTO, userCreationDTO.RoleId, token).Result;
        }

        public UserDTO UpdateUser(UserUpdateDTO userUpdateDTO, string token)
        {
            CustomValidatorInput<UserUpdateDTO>.DTOValidator(userUpdateDTO);
            return _usersRepository.UpdateUser(userUpdateDTO, token).Result;
        }

        public UserDTO DeleteUser(int id, string token)
        {
            return _usersRepository.DeleteUser(id, token).Result;
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