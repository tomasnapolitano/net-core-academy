using Models.DTOs.Service;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task<List<ServiceDTO>> GetServices();
        Task<List<ServiceTypeDTO>> GetServiceTypes();
        Task<ServiceDTO> PostService(ServiceCreationDTO serviceCreationDTO);
    }
}
