using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.User;
using Models.Entities;
using Repositories.Interfaces;

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

        public async Task<int> GetRoleById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if(user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario");
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

        public async Task<List<UserDTO>> GetUsersWithFullName()
        {
            var users = await _context.Users.Select(x => new UserDTO()
                                            {
                                                UserId = x.UserId,
                                                RoleId = x.RoleId,
                                                AdressId = x.AdressId,
                                                FirstName = x.FirstName,
                                                LastName = x.LastName,
                                                Email = x.Email,
                                                Dninumber = x.Dninumber,
                                                CreationDate = x.CreationDate,
                                                FullName = x.FirstName + ' ' + x.LastName
                                            })
                                            .ToListAsync();

            return users;
        }

        public async Task<UserDTO> PostUser(UserCreationDTO userCreationDTO , int userRole)
        {
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
                AdressId = user.AdressId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Dninumber = user.Dninumber,
                CreationDate = user.CreationDate,

            };
            return userDTO;

        }
    }
}