using Models.DTOs.District;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;

        public DistrictService(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public List<DistrictDTO> GetDistricts()
        {
            return _districtRepository.GetDistricts().Result;
        }

        public DistrictDTO GetDistrictById(int id)
        {
            return _districtRepository.GetDistrictById(id).Result;
        }

        public DistrictWithServicesDTO GetDistrictWithServices(int districtId)
        {
            return _districtRepository.GetDistrictWithServices(districtId).Result;
        }

        public DistrictAgentDTO GetDistrictsWithAgent(int districtId)
        {
            return _districtRepository.GetDistrictsWithAgent(districtId).Result;
        }

        public bool AddAgentToDistrict(int agentId, int districtId)
        {
            return _districtRepository.AddAgentToDistrict(agentId, districtId).Result;
        }

        public bool RemoveAgentFromDistrict(int districtId)
        {
            return _districtRepository.RemoveAgentFromDistrict(districtId).Result;
        }

        public DistrictWithServicesDTO AddServiceToDistrict(int districtId, int serviceId)
        {
            return _districtRepository.AddServiceToDistrict(districtId, serviceId).Result;
        }

        public DistrictWithServicesDTO DeactivateServiceByDistrict(int districtId, int serviceId)
        {
            return _districtRepository.DeactivateServiceByDistrict(districtId, serviceId).Result;
        }
    }
}