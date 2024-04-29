using Models.DTOs.Service;

namespace Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task<List<ServiceDTO>> GetServices();
        Task<ServiceDTO> GetServiceById(int id);
        Task<List<ServiceTypeDTO>> GetServiceTypes();
        Task<ServiceTypeDTO> GetServiceTypeById(int id);
        Task<ServiceDTO> PostService(ServiceCreationDTO serviceCreationDTO);
        Task<ServiceDTO> UpdateService(ServiceUpdateDTO serviceDTO);
        Task<bool> DeleteService(int id);
        Task<ServiceDTO> ActiveService(int id);
    }
}