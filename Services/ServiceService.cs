using Models.DTOs.Service;
using Models.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
