using Models.Entities;

namespace Repositories.Interfaces
{
    public interface IDistrictRepository
    {
        Task<List<District>> GetDistricts();
    }
}