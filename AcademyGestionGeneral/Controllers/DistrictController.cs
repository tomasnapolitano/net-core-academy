using Microsoft.AspNetCore.Mvc;
using Models.DTOs.District;
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

        // GET: api/District/5
        /// <summary>
        /// Obtiene un distrito
        /// </summary>
        /// <param name="id">Es el id del distrito</param>
        /// <returns>DistrictDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("{id}")]
        public DistrictDTO GetDistrictById(int id)
        {
            return _districtService.GetDistrictById(id);
        }

        // GET: api/District/Agents/5
        /// <summary>
        /// Obtiene un distrito con su respectivo agente
        /// </summary>
        /// <param name="districtId">Es el id del distrito</param>
        /// <returns>DistrictAgentDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("Agents/{districtId}")]
        public DistrictAgentDTO GetDistrictsWithAgent(int districtId)
        {
            return _districtService.GetDistrictsWithAgent(districtId);
        }

        // PUT: api/District/AddAgentToDistrict/{agentId}/{districtId}
        /// <summary>
        /// Asigna un agente a un distrito en especifico
        /// </summary>
        /// <param name="agentId">Es el id del agente</param>
        /// <param name="districtId">Es el id del distrito</param>
        /// <returns>DistrictDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPut("AddAgentToDistrict/{agentId}/{districtId}")]
        public bool AddAgentToDistrict(int agentId, int districtId)
        {
            return _districtService.AddAgentToDistrict(agentId, districtId);
        }

        // PUT: api/District/RemoveAgentFromDistrict/{districtId}
        /// <summary>
        /// Desasigna un agente a un distrito en especifico
        /// </summary>
        /// <param name="districtId">Es el id del distrito</param>
        /// <returns>DistrictDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPut("RemoveAgentFromDistrict/{districtId}")]
        public bool RemoveAgentFromDistrict(int districtId)
        {
            return _districtService.RemoveAgentFromDistrict(districtId);
        }
    }
}