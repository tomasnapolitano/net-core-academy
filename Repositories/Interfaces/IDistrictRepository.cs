using Models.DTOs.District;
using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IDistrictRepository
    {
        Task<List<DistrictDTO>> GetDistricts();
        Task<List<LocationDTO>> GetLocations();
        Task<DistrictDTO> GetDistrictById(int id);
        Task<DistrictAgentDTO> GetDistrictsWithAgent(int districtId);
        Task<DistrictWithServicesDTO> GetDistrictWithServices(int districtId);
        Task<bool> AddAgentToDistrict(int agentId, int districtId);
        Task<bool> RemoveAgentFromDistrict(int districtId);
        Task<DistrictWithServicesDTO> AddServiceToDistrict(int districtId, int serviceId);
        Task<DistrictXservice> PostDistrictXservice(int districtId, int serviceId);
        Task<DistrictWithServicesDTO> DeactivateServiceByDistrict(int districtId, int serviceId);
    }
}