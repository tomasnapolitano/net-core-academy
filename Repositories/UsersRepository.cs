using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ManagementServiceContext _context;

        public UsersRepository(ManagementServiceContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }
    }
}