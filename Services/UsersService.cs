using Microsoft.IdentityModel.Tokens;
using Models.DTOs;
using Models.DTOs.Bill;
using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;
using Models.Entities;
using QuestPDF.Fluent;
using Repositories.Interfaces;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utils.CustomValidator;

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

        public bool ChangePassword(UserUpdatePasswordDTO userUpdatePassDTO)
        {
            return _usersRepository.ChangePassword(userUpdatePassDTO).Result;
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

        public List<UserWithServicesDTO> GetUsersWithServices()
        {
            return _usersRepository.GetUsersWithServices().Result;
        }

        public UserWithServicesDTO GetUserWithServicesById(int userId)
        {
            return _usersRepository.GetUserWithServicesById(userId).Result;
        }

        public ServiceSubscriptionWithUserDTO GetServiceSubscriptionClient(int subscriptionId)
        {
            return _usersRepository.GetSubscription(subscriptionId).Result;
        }

        public ConsumptionDTO GetRandomSubscriptionConsumption(int subscriptionId)
        {
            return _usersRepository.GetRandomSubscriptionConsumption(subscriptionId).Result;
        }

        public ConsumptionBillDTO GenerateBill(int userId)
        {
            return _usersRepository.GenerateBill(userId).Result;
        }

        public ConsumptionBillDTO GetBillById(int billId)
        {
            return _usersRepository.GetBillById(billId).Result;
        }

        public List<ConsumptionBillDTO> GetBillsByUserId(int userId)
        {
            return _usersRepository.GetBillsByUserId(userId).Result;
        }

        public List<ConsumptionBillDTO> GetAllBills()
        {
            return _usersRepository.GetAllBills().Result;
        }

        public Stream GetBillPdf(int billId)
        {
            ConsumptionBillDTO billDTO = _usersRepository.GetBillById(billId).Result;
            ConsumptionBillPdf billPdf = new ConsumptionBillPdf(billDTO);
            byte[] pdfByteArray = billPdf.GeneratePdf();
            MemoryStream pdfStream = new MemoryStream(pdfByteArray);

            return pdfStream;
        }

        public UserDTO PostUser(UserCreationDTO userCreationDTO, string token)
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