using Models.DTOs.Service;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        List<ServiceDTO> GetServices();
        ServiceDTO GetServiceById(int id);
        List<ServiceTypeDTO> GetServiceTypes();
        ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO);
    }
}