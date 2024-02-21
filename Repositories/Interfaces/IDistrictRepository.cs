using Models.DTOs.District;

namespace Repositories.Interfaces
{
    public interface IDistrictRepository
    {
        Task<List<DistrictDTO>> GetDistricts();
        Task<DistrictDTO> GetDistrictById(int id);
        Task<DistrictAgentDTO> GetDistrictsWithAgent(int districtId);
        Task<bool> AddAgentToDistrict(int agentId, int districtId);
        Task<bool> RemoveAgentFromDistrict(int districtId);
    }
}