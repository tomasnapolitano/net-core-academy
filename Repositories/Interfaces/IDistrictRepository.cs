using Models.DTOs.District;
using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IDistrictRepository
    {
        Task<List<DistrictDTO>> GetDistricts();
        Task<DistrictDTO> GetDistrictById(int id);
    }
}