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

        public ServiceDTO GetServiceById(int id)
        {
            return _serviceRepository.GetServiceById(id).Result;
        }

        public ServiceDTO UpdateService(ServiceUpdateDTO serviceUpdateDTO)
        {
            return _serviceRepository.UpdateService(serviceUpdateDTO).Result;
        }

        public bool DeleteService(int id)
        {
            return _serviceRepository.DeleteService(id).Result;
        }

        public List<ServiceTypeDTO> GetServiceTypes()
        {
            return _serviceRepository.GetServiceTypes().Result;
        }

        public ServiceTypeDTO GetServiceTypeById(int id)
        {
            return _serviceRepository.GetServiceTypeById(id).Result;
        }

        public ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO)
        {
            return _serviceRepository.PostService(serviceCreationDTO).Result;
        }
    }
}