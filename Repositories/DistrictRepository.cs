using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly ManagementServiceContext _context;

        public DistrictRepository(ManagementServiceContext context)
        {
            _context = context;
        }

        public async Task<List<District>> GetDistricts()
        {
            var districts = await _context.Districts.ToListAsync();

            return districts;
        }
    }
}