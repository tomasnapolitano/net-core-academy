using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.User;
using Models.Entities;
using Repositories.Interfaces;
using Utils.Enum;
using Utils.Middleware;

namespace Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ManagementServiceContext _context;
        private readonly IMapper _mapper;

        public UsersRepository(ManagementServiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            if (users.Count == 0)
            {
                throw new KeyNotFoundException("La lista de usuarios está vacía.");
            }

            return _mapper.Map<List<UserDTO>>(users);
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

        public async Task<List<UserDTO>> GetUsersWithFullName()
        {
            var users = await _context.Users.Select(x => new UserDTO()
                                            {
                                                UserId = x.UserId,
                                                RoleId = x.RoleId,
                                                AddressId = x.AdressId,
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
                AdressId = addres.AddressId,
                FirstName = userCreationDTO.FirstName,
                LastName = userCreationDTO.LastName,
                Email = userCreationDTO.Email,
                Dninumber = userCreationDTO.Dninumber,
                Password = userCreationDTO.Password,
                CreationDate = DateTime.Now,
            };

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var userDTO = new UserDTO()
            {
                UserId = user.UserId,
                RoleId = user.RoleId,
                AddressId = user.AdressId,
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