using Models.DTOs.Service;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class ServiceService : IServiceService
    {
        public readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public List<ServiceDTO> GetServices()
        {
            return _serviceRepository.GetServices().Result;
        }

        public List<ServiceTypeDTO> GetServiceTypes()
        {
            return _serviceRepository.GetServiceTypes().Result;
        }

        public ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO)
        {
            return _serviceRepository.PostService(serviceCreationDTO).Result;
        }
    }
}