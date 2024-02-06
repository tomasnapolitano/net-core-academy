using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using Repositories.Interfaces;
using Utils.Middleware;

namespace Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ManagementServiceContext _context;

        public UsersRepository(ManagementServiceContext context)
        {
            _context = context;
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

        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

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

            //acordate de encriptar pass
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
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Dninumber = user.Dninumber

            };
            return userDTO;

        }
    }
}