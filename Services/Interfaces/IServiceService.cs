using Models.DTOs.Service;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        List<ServiceDTO> GetServices();
        ServiceDTO GetServiceById(int id);
        List<ServiceTypeDTO> GetServiceTypes();
        ServiceTypeDTO GetServiceTypeById(int id);
        ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO);
        ServiceDTO UpdateService(ServiceUpdateDTO serviceUpdateDTO);
        bool DeleteService(int id);
        ServiceDTO ActiveService(int id);
    }
}