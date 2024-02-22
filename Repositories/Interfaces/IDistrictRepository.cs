using Models.DTOs.District;
using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IDistrictRepository
    {
        Task<List<DistrictDTO>> GetDistricts();
        Task<DistrictDTO> GetDistrictById(int id);
        Task<DistrictAgentDTO> GetDistrictsWithAgent(int districtId);
        Task<bool> AddAgentToDistrict(int agentId, int districtId);
        Task<bool> RemoveAgentFromDistrict(int districtId);
        Task<bool> AddServiceToDistrict(int districtId, int serviceId);
        Task<DistrictXservice> PostDistrictXservice(int districtId, int serviceId);
    }
}