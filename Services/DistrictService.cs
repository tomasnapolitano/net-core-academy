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
    }
}