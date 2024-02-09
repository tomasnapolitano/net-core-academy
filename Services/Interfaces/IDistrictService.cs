using Models.DTOs.District;

namespace Services.Interfaces
{
    public interface IDistrictService
    {
        List<DistrictDTO> GetDistricts();
        DistrictDTO GetDistrictById(int id);
        DistrictAgentDTO GetDistrictsWithAgent(int districtId);
    }
}