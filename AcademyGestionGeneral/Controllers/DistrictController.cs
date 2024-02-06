using Microsoft.AspNetCore.Mvc;
using Models.DTOs.District;
using Models.Entities;
using Services.Interfaces;

namespace AcademyGestionGeneral.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        // GET: api/District
        /// <summary>
        /// Obtiene todos los distritos
        /// </summary>
        /// <returns>Lista de Distritos</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet]
        public List<DistrictDTO> GetDistricts()
        {
            return _districtService.GetDistricts();
        }
    }
}