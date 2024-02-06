using Models.DTOs.District;

namespace Services.Interfaces
{
    public interface IDistrictService
    {
        List<DistrictDTO> GetDistricts();
    }
}