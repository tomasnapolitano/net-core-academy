using Microsoft.EntityFrameworkCore;
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

        public ServiceRepository(ManagementServiceContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            return services;
        }
    }
}
