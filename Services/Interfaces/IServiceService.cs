using Models.DTOs.Service;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        List<ServiceDTO> GetServices();
        List<ServiceTypeDTO> GetServiceTypes();
        ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO);
    }
}