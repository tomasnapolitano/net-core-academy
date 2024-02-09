using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Service;
using Models.Entities;
using Repositories.Interfaces;

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

        public async Task<ServiceDTO> GetServiceById(int id)
        {
            var service = await _context.Services.Where(x => x.ServiceId == id).FirstOrDefaultAsync();

            if (service == null)
            {
                throw new KeyNotFoundException($"No se encontró servicio con el Id: {id}");
            }

            return _mapper.Map<ServiceDTO>(service);
        }

        public async Task<ServiceDTO> UpdateService(ServiceUpdateDTO serviceUpdateDTO)
        {
            var updatedService = _mapper.Map<Service>(serviceUpdateDTO);
            var existingService = await _context.Services.FindAsync(updatedService.ServiceId);

            if (existingService == null)
            {
                throw new KeyNotFoundException($"No se encontró servicio con el Id: {serviceUpdateDTO.ServiceId}");
            }

            _context.Entry(existingService).CurrentValues.SetValues(updatedService);
            await _context.SaveChangesAsync();

            return _mapper.Map<ServiceDTO>(updatedService);
        }

        public async Task<List<ServiceTypeDTO>> GetServiceTypes()
        {
            var serviceTypes = await _context.ServiceTypes.ToListAsync();

            if (serviceTypes.Count == 0)
            {
                throw new KeyNotFoundException("La lista de tipos de servicios está vacía.");
            }

            return _mapper.Map<List<ServiceTypeDTO>>(serviceTypes);
        }

        public async Task<ServiceTypeDTO> GetServiceTypeById(int id)
        {
            var serviceType = await _context.ServiceTypes.Where(x => x.ServiceTypeId == id).FirstOrDefaultAsync();

            if (serviceType == null)
            {
                throw new KeyNotFoundException($"No se encontró tipo de servicio con el Id: {id}");
            }

            return _mapper.Map<ServiceTypeDTO>(serviceType);
        }

        public async Task<ServiceDTO> PostService(ServiceCreationDTO serviceCreationDTO)
        {
            var serviceType = await _context.ServiceTypes.Where(x =>
                                        x.ServiceTypeId == serviceCreationDTO.ServiceTypeId)
                                        .FirstOrDefaultAsync();
            if (serviceType == null)
            {
                throw new KeyNotFoundException($"No se encontró tipo de servicio con el Id: {serviceCreationDTO.ServiceTypeId}");
            }

            var service = _mapper.Map<Service>(serviceCreationDTO);
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();

            return _mapper.Map<ServiceDTO>(service);
        }
    }
}