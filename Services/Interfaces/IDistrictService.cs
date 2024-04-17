using Models.DTOs.District;

namespace Services.Interfaces
{
    public interface IDistrictService
    {
        List<DistrictDTO> GetDistricts();
        List<LocationDTO> GetLocations();
        DistrictDTO GetDistrictById(int id);
        DistrictAgentDTO GetDistrictsWithAgent(int districtId);
        bool AddAgentToDistrict(int agentId, int districtId);
        bool RemoveAgentFromDistrict(int districtId);
        DistrictWithServicesDTO AddServiceToDistrict(int districtId, int serviceId);
        DistrictWithServicesDTO GetDistrictWithServices(int districtId);
        DistrictWithServicesDTO DeactivateServiceByDistrict(int districtId, int serviceId);
        Dictionary<string, Dictionary<string, int>> GetServicesByDistrictReport();
    }
}