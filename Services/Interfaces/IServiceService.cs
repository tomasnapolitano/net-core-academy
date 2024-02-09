using Models.DTOs.Service;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        List<ServiceDTO> GetServices();
        ServiceDTO GetServiceById(int id);
        ServiceDTO UpdateService(ServiceUpdateDTO serviceUpdateDTO);
        List<ServiceTypeDTO> GetServiceTypes();
        ServiceTypeDTO GetServiceTypeById(int id);
        ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO);
    }
}