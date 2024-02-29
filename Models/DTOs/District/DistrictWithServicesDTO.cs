using Models.DTOs.Service;

namespace Models.DTOs.District
{
    public class DistrictWithServicesDTO : DistrictDTO
    {
        public IList<ServiceDTO> Services { get; set; }
    }
}