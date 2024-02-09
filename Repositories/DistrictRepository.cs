using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.District;
using Models.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly ManagementServiceContext _context;
        private readonly IMapper _mapper;

        public DistrictRepository(ManagementServiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DistrictDTO>> GetDistricts()
        {
            var districts = await _context.Districts.ToListAsync();

            if (districts.Count == 0)
            {
                throw new KeyNotFoundException("La lista de distritos está vacía.");
            }

            return _mapper.Map<List<DistrictDTO>>(districts);
        }

        public async Task<DistrictDTO> GetDistrictById(int id)
        {
            var district = await _context.Districts
                                        .Include(d => d.Agent) 
                                        .FirstOrDefaultAsync(x => x.DistrictId == id);

            if (district == null)
            {
                throw new KeyNotFoundException("No se encontró el distrito.");
            }

            return _mapper.Map<DistrictDTO>(district);
        }

    }
}