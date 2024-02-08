using Models.DTOs.Service;

namespace Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task<List<ServiceDTO>> GetServices();
        Task<List<ServiceTypeDTO>> GetServiceTypes();
        Task<ServiceTypeDTO> GetServiceTypeById(int id);
        Task<ServiceDTO> PostService(ServiceCreationDTO serviceCreationDTO);
    }
}