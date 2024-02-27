using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.User;
using Models.Entities;
using Repositories.Interfaces;
using Repositories.Utils.PasswordHasher;
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

        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            if (users.Count == 0)
            {
                throw new KeyNotFoundException("La lista de usuarios está vacía.");
            }

            return _mapper.Map<List<UserDTO>>(users);
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

        public async Task<List<UserDTO>> GetUsersWithFullName()
        {
            var users = await _context.Users.Select(x => new UserDTO()
            {
                UserId = x.UserId,
                RoleId = x.RoleId,
                AddressId = x.AddressId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                DniNumber = x.Dninumber,
                CreationDate = x.CreationDate,
                FullName = x.FirstName + ' ' + x.LastName
            })
                                            .ToListAsync();

            return users;
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

            int? districtId = user.Address.Location.DistrictId;
            var districtXservice = await _context.DistrictXservices.FirstOrDefaultAsync(
                                                        dxs => dxs.DistrictId == districtId 
                                                        && dxs.ServiceId == serviceId);

            if (districtXservice == null || districtXservice.Active == false)
            {
                throw new KeyNotFoundException("Este servicio no se encuentra disponible para este usuario.");
            }

            // Creando la suscripción:
            ServiceSubscription subscription = new ServiceSubscription()
            {
                UserId = userId,
                DistrictXserviceId = districtXservice.DistrictXserviceId,
                StartDate = DateTime.Now,
                PauseSubscription = false,
            };

            await _context.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task<UserWithServicesDTO> GetUserWithServices(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new KeyNotFoundException("No se encontró el usuario.");

            var userDTO = _mapper.Map<UserDTO>(user);
            var subscriptionQueryResult = await _context.ServiceSubscriptions
                                                        .Include(s => s.DistrictXservice)
                                                        .ThenInclude(dxs => dxs.Service)
                                                        .Select(x => x.UserId == userId 
                                                            && x.DistrictXservice.Active == true 
                                                            && x.DistrictXservice.Service.Active == true)
                                                        .ToListAsync();


        }

        public async Task<UserDTO> PostUser(UserCreationDTO userCreationDTO , int userRole)
        {
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

        public async Task<UserDTO> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
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

        public async Task<UserDTO> DeleteUser(int id)
        {
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
    }
}