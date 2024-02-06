using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.District;
using Models.DTOs.Service;
using Models.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ManagementServiceContext _context;
        private readonly IMapper _mapper;

        public ServiceRepository(ManagementServiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ServiceDTO>> GetServices()
        {
            var services = await _context.Services.ToListAsync();

            if (services.Count == 0)
            {
                throw new KeyNotFoundException("La lista de servicios está vacía.");
            }

            return _mapper.Map<List<ServiceDTO>>(services);
        }
    }
}
