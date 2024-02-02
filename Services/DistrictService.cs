using Models.Entities;
using Repositories;
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

        public List<District> GetDistricts()
        {
            return _districtRepository.GetDistricts().Result;
        }
    }
}