using Models.DTOs.Service;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        List<ServiceDTO> GetServices();
        List<ServiceTypeDTO> GetServiceTypes();
        ServiceTypeDTO GetServiceTypeById(int id);
        ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO);
    }
}