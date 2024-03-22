using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;
using Models.Entities;
using Repositories.Interfaces;
using Repositories.Utils.PasswordHasher;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Utils.Enum;
using Utils.Middleware;

namespace Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ManagementServiceContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UsersRepository(ManagementServiceContext context, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserWithTokenDTO> Login(UserLoginDTO userLoginDTO)
        {
            var searchedUser = await _context.Users.Where(u => 
                                                 u.Email == userLoginDTO.Email)
                                             .FirstOrDefaultAsync();

            if (searchedUser == null)
                throw new KeyNotFoundException("El email ingresado no tiene una cuenta asociada. Por favor comuníquese con su Agente asignado para dar de alta su cuenta.");
            
            if (!_passwordHasher.Verify(searchedUser.Password, userLoginDTO.Password))
                throw new BadRequestException("La contraseña ingresada no es correcta.");

            // crear una custom exception para error de login?

            return _mapper.Map<UserWithTokenDTO>(searchedUser);
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await _context.Users.Include(u => u.Address)
                                            .ThenInclude(a => a.Location)
                                            .ThenInclude(l => l.District)
                                            .ToListAsync();

            if (users.Count == 0)
            {
                throw new KeyNotFoundException("La lista de usuarios está vacía.");
            }

            List<UserDTO> usersDTO = new List<UserDTO>();

            foreach(var user in users)
            {
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                userDTO.District.DistrictId = user.Address.Location.DistrictId;
                userDTO.District.DistrictName = user.Address.Location.District.DistrictName;
                usersDTO.Add(userDTO);
            }

            return usersDTO;
        }

        public async Task<List<UserDTO>> GetActiveUsers()
        {
            var users = await _context.Users.Where(x => x.Active == true).ToListAsync();

            if (users.Count == 0)
            {
                throw new KeyNotFoundException("La lista de usuarios activos está vacía.");
            }

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetAllAgent()
        {
            var listAgent = await _context.Users.Where(x=> x.RoleId == (int)UserRoleEnum.Agent).ToListAsync();

            if (listAgent.Count == 0)
            {
                throw new KeyNotFoundException("No se encontraron agentes en el sistema");
            }

            return _mapper.Map<List<UserDTO>>(listAgent);
        }

        public async Task<int> GetRoleById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if(user == null)
            {
                throw new KeyNotFoundException($"No se encontró el usuario con rol id igual a :{id}");
            }

            return user.RoleId;
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetUsersByDistrictId(int districtId)
        {
            var district = await _context.Districts
                                        .Include(d => d.Locations)
                                        .ThenInclude(l => l.Addresses)
                                        .ThenInclude(a => a.Users)
                                        .FirstOrDefaultAsync(d => d.DistrictId == districtId);

            if (district == null)
            {
                throw new KeyNotFoundException($"No se encontró el distrito con ID {districtId}.");
            }

            var users = district.Locations
                                .SelectMany(l => l.Addresses)
                                .SelectMany(a => a.Users)
                                .ToList();

            if (users.Count == 0)
            {
                throw new KeyNotFoundException("No se encontraron usuarios en el distrito especificado.");
            }

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<AgentDTO> GetAgentsWithDistrict(int agentId)
        {
            var user = await _context.Users
                                    .Include(d => d.Districts)
                                    .FirstOrDefaultAsync(x => x.UserId == agentId);

            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }

            if (user.RoleId != (int)UserRoleEnum.Agent)
            {
                throw new BadRequestException("El usuario no posee rol de agente.");
            }

            return _mapper.Map<AgentDTO>(user);
        }

        public async Task<UserWithServicesDTO> SubscribeUserToService(int userId, int serviceId)
        {
            var user = await _context.Users.Include(u => u.Address)
                                            .ThenInclude(a => a.Location)
                                            .ThenInclude(l => l.District)
                                            .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }
            if (user.Address.Location == null)
            {
                throw new KeyNotFoundException("El usuario no tiene una locación asignada.");
            }
            if (user.Address.Location.District == null)
            {
                throw new KeyNotFoundException("El usuario no tiene distrito asignado.");
            }

            var service = await _context.Services.FindAsync(serviceId);

            if (service == null)
            {
                throw new KeyNotFoundException("No se encontró el servicio.");
            }
            if (service.Active == false)
            {
                throw new KeyNotFoundException("Este servicio se encuentra deshabilitado.");
            }

            int? districtId = user.Address.Location.DistrictId;
            var districtXservice = await _context.DistrictXservices.FirstOrDefaultAsync(
                                                        dxs => dxs.DistrictId == districtId
                                                        && dxs.ServiceId == serviceId);

            if (districtXservice == null || districtXservice.Active == false)
            {
                throw new KeyNotFoundException("Este servicio no se encuentra disponible para este usuario.");
            }

            var existingSubscription = await _context.ServiceSubscriptions.FirstOrDefaultAsync(
                                                        s => s.UserId == userId
                                                        && s.DistrictXserviceId == districtXservice.DistrictXserviceId);

            if (existingSubscription != null)
            { // Ya está suscrito:
                return await GetUserWithServices(userId);
            }

            // No está suscrito. Creando la suscripción:
            ServiceSubscription subscription = new ServiceSubscription()
            {
                UserId = userId,
                DistrictXserviceId = districtXservice.DistrictXserviceId,
                StartDate = DateTime.Now,
                PauseSubscription = false,
            };

            await _context.AddAsync(subscription);
            await _context.SaveChangesAsync();

            return await GetUserWithServices(userId);
        }

        public async Task<UserWithServicesDTO> PauseSubscribeUserToService(int subscriptionId)
        {
            var subscription = await _context.ServiceSubscriptions.FirstOrDefaultAsync(idS => idS.SubscriptionId == subscriptionId);

            if (subscription == null)
            {
                throw new KeyNotFoundException("No se encontró ninguna suscripción con el ID indicado.");
            }

            if(subscription.UserId == null)
            {
                throw new KeyNotFoundException("La suscripción no está relacionada a ningun usuario.");
            }

            if (subscription.PauseSubscription == true)
            {
                throw new BadRequestException("La suscripción del ID ingresado ya está pausada.");
            }

            // Chequeamos si es agente? si es su suscripción?

            subscription.PauseSubscription = true;

            _context.Entry(subscription)
                .Property(x => x.PauseSubscription).IsModified = true;
            await _context.SaveChangesAsync();

            return await GetUserWithServices((int)subscription.UserId);
        }

        public async Task<UserWithServicesDTO> GetUserWithServices(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new KeyNotFoundException("No se encontró el usuario.");

            var userWithServicesDTO = _mapper.Map<UserWithServicesDTO>(user);
            var subscriptionQueryResult = await _context.ServiceSubscriptions
                                                        .Include(s => s.DistrictXservice)
                                                        .ThenInclude(dxs => dxs.Service)
                                                        .Where(x => x.UserId == userId 
                                                            && x.DistrictXservice.Active == true 
                                                            && x.DistrictXservice.Service.Active == true)
                                                        .ToListAsync();
            foreach (var subscription in subscriptionQueryResult)
            {
                ServiceDTO serviceDTO = new ServiceDTO()
                {
                    ServiceId = subscription.DistrictXservice.Service.ServiceId,
                    ServiceTypeId = subscription.DistrictXservice.Service.ServiceTypeId,
                    ServiceName = subscription.DistrictXservice.Service.ServiceName,
                    PricePerUnit = subscription.DistrictXservice.Service.PricePerUnit
                };

                ServiceSubscriptionDTO serviceSubscriptionDTO = new ServiceSubscriptionDTO()
                {
                    SubscriptionId = subscription.SubscriptionId,
                    UserId = subscription.UserId,
                    DistrictXserviceId = subscription.DistrictXservice.DistrictXserviceId,
                    StartDate = subscription.StartDate,
                    PauseSubscription = subscription.PauseSubscription,
                    Service = serviceDTO
                };
                userWithServicesDTO.ServiceSubscriptions.Add(serviceSubscriptionDTO);
            }

            return userWithServicesDTO;
        }

        public async Task<UserDTO> PostUser(UserCreationDTO userCreationDTO , int userRole, string token)
        {
            var rol = GetRolesFromToken(token);
            var clientRoleValue = Convert.ToInt32(UserRoleEnum.Client).ToString();
            var agentRoleValue = Convert.ToInt32(UserRoleEnum.Agent).ToString();

            if (rol.Contains(clientRoleValue) || (rol.Contains(agentRoleValue) && userCreationDTO.RoleId == (int)UserRoleEnum.Admin))
            {
                throw new UnauthorizedAccessException("El usuario no tiene permisos para acceder a este recurso.");
            }

            if (ExistsDniUser(userCreationDTO.Dninumber).Result)
            {
                throw new KeyNotFoundException($"El DNI ingresado ya existe, no puede crear un usuario con DNI: {userCreationDTO.Dninumber}");
            }

            if (ExistsEmailUser(userCreationDTO.Email).Result)
            {
                throw new KeyNotFoundException($"El Email ingresado ya existe, no puede crear un usuario con Email: {userCreationDTO.Email}");
            }

            if (!ExistsUserRole(userCreationDTO.RoleId).Result)
            {
                throw new KeyNotFoundException($"No se puede crear un usuario con un Id Role que no existe en el sistema: {userCreationDTO.RoleId}");
            }

            var location = await _context.Locations.Where(x => x.PostalCode == userCreationDTO.PostalCode && x.DistrictId == userCreationDTO.DistrictId).FirstOrDefaultAsync();

            if (location == null)
            {
                throw new KeyNotFoundException($"No se encontró localidad con el codigo postal: {userCreationDTO.PostalCode}");
            }

            var addres = new Address()
            {
                StreetName = userCreationDTO.StreetName,
                Neighborhood = userCreationDTO.Neighborhood,
                StreetNumber = userCreationDTO.StreetNumber,
                LocationId = location.LocationId
            };

            await _context.AddAsync(addres);
            await _context.SaveChangesAsync();

            var user = new User() 
            {
                RoleId = userRole,
                AddressId = addres.AddressId,
                FirstName = userCreationDTO.FirstName,
                LastName = userCreationDTO.LastName,
                Email = userCreationDTO.Email,
                Dninumber = userCreationDTO.Dninumber,
                Password = _passwordHasher.Hash(userCreationDTO.Password),
                CreationDate = DateTime.Now,
            };

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var userDTO = new UserDTO()
            {
                UserId = user.UserId,
                RoleId = user.RoleId,
                AddressId = user.AddressId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DniNumber = user.Dninumber,
                CreationDate = user.CreationDate,

            };
            return userDTO;
        }

        public async Task<UserDTO> UpdateUser(UserUpdateDTO userUpdateDTO, string token)
        {
            var rol = GetRolesFromToken(token);
            var clientRoleValue = Convert.ToInt32(UserRoleEnum.Client).ToString();

            if (rol.Contains(clientRoleValue))
            {
                throw new UnauthorizedAccessException("El usuario no tiene permisos para acceder a este recurso.");
            }

            var existingUser = await _context.Users.FindAsync(userUpdateDTO.UserId);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("No se encontró un usuario con el Id ingresado.");
            }

            existingUser.FirstName = userUpdateDTO.FirstName;
            existingUser.LastName = userUpdateDTO.LastName;
            existingUser.Email = userUpdateDTO.Email;

            await _context.SaveChangesAsync();

            return await GetUserById(existingUser.UserId);
        }

        public async Task<UserDTO> DeleteUser(int id, string token)
        {
            var rol = GetRolesFromToken(token);
            var adminRoleValue = Convert.ToInt32(UserRoleEnum.Admin).ToString();

            if (!rol.Contains(adminRoleValue))
            {
                throw new UnauthorizedAccessException("El usuario no tiene permisos para acceder a este recurso.");
            }

            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("No se encontró un usuario con el Id ingresado.");
            }

            existingUser.Active = false;
            await _context.SaveChangesAsync();

            return await GetUserById(existingUser.UserId);
        }

        private async Task<bool> ExistsDniUser(string dni)
        {
            return await _context.Users.AnyAsync(x=> x.Dninumber == dni);
        }
        private async Task<bool> ExistsEmailUser(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }
        private async Task<bool> ExistsUserRole(int role)
        {
            return await _context.UserRoles.AnyAsync(x => x.RoleId == role);
        }
        private string[] GetRolesFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new InvalidOperationException("El token JWT no pudo ser leído correctamente.");
            }

            var roles = jsonToken?.Claims
                                .Where(claim => claim.Type == "role")
                                .Select(claim => claim.Value)
                                .ToArray();

            return roles ?? new string[0];
        }
    }
}