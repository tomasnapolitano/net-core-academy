using Models.DTOs.Service;
using Models.Entities;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        List<ServiceDTO> GetServices();
        List<ServiceTypeDTO> GetServiceTypes();
        ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO);
    }
}