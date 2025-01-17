using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.District;
using Services.Interfaces;

namespace AcademyGestionGeneral.Controllers
{
    [Authorize]
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
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet]
        public List<DistrictDTO> GetDistricts()
        {
            return _districtService.GetDistricts();
        }

        // GET: api/District/Locations
        /// <summary>
        /// Obtiene todas las localidades
        /// </summary>
        /// <returns>Lista de Localidades</returns>
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("locations")]
        public List<LocationDTO> GetLocations()
        {
            return _districtService.GetLocations();
        }

        // GET: api/District/5
        /// <summary>
        /// Obtiene un distrito
        /// </summary>
        /// <param name="id">Es el id del distrito</param>
        /// <returns>DistrictDTO</returns>
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("{id}")]
        public DistrictDTO GetDistrictById(int id)
        {
            return _districtService.GetDistrictById(id);
        }

        // GET: api/District/{districtId}/services
        /// <summary>
        /// Obtiene un distrito con sus servicios disponibles
        /// </summary>
        /// <param name="districtId">Es el id del distrito</param>
        /// <returns>DistrictWithServicesDTO</returns>
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("{districtId}/services")]
        public DistrictWithServicesDTO GetDistrictWithServices(int districtId)
        {
            return _districtService.GetDistrictWithServices(districtId);
        }

        // GET: api/District/Agents/5
        /// <summary>
        /// Obtiene un distrito con su respectivo agente
        /// </summary>
        /// <param name="districtId">Es el id del distrito</param>
        /// <returns>DistrictAgentDTO</returns>
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("Agents/{districtId}")]
        public DistrictAgentDTO GetDistrictsWithAgent(int districtId)
        {
            return _districtService.GetDistrictsWithAgent(districtId);
        }

        // GET: api/District/info
        /// <summary>
        /// Obtiene una lista de distritos con su/s agente/s y los servicios que brinda
        /// </summary>
        /// <returns>List<DistrictInfoDTO></returns>
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("info")]
        public List<DistrictInfoDTO> GetAllDistrictsInfo()
        {
            return _districtService.GetAllDistrictsInfo();
        }

        // PUT: api/District/AddAgentToDistrict/{agentId}/{districtId}
        /// <summary>
        /// Asigna un agente a un distrito en especifico
        /// </summary>
        /// <param name="agentId">Es el id del agente</param>
        /// <param name="districtId">Es el id del distrito</param>
        /// <returns>DistrictDTO</returns>
        /// <response code="200">La operaci�n fue exitosa</response>
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
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPut("RemoveAgentFromDistrict/{districtId}")]
        public bool RemoveAgentFromDistrict(int districtId)
        {
            return _districtService.RemoveAgentFromDistrict(districtId);
        }

        // POST: api/District/{districtId}/services/add/{serviceId}
        /// <summary>
        /// Agrega un servicio existente a un distrito en espec�fico
        /// </summary>
        /// <param name="districtId">Es el id del distrito</param>
        /// <param name="serviceId">Es el id del servicio</param>
        /// <returns>DistrictWithServiceDTO</returns>
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPost("{districtId}/services/add")]
        public DistrictWithServicesDTO AddServiceToDistrict(int districtId, [FromBody]int serviceId)
        {
            return _districtService.AddServiceToDistrict(districtId, serviceId);
        }

        // PUT: api/District/{districtId}/services/deactivate/{serviceId}
        /// <summary>
        /// Desactiva un servicio existente de un distrito en espec�fico
        /// </summary>
        /// <param name="districtId">Es el id del distrito</param>
        /// <param name="serviceId">Es el id del servicio</param>
        /// <returns>DistrictWithServicesDTO</returns>
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPut("{districtId}/services/deactivate")]
        public DistrictWithServicesDTO DeactivateServiceByDistrict(int districtId, [FromBody]int serviceId)
        {
            return _districtService.DeactivateServiceByDistrict(districtId, serviceId);
        }

        // GET: api/District/reports/servicesByDistrict
        /// <summary>
        /// Servicios disponibles junto a cantidad de usuarios suscriptos en ese distrito
        /// </summary>
        /// <returns></returns>
        /// <response code="200">La operaci�n fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("reports/servicesByDistrict")]
        public Dictionary<string, Dictionary<string, int>> GetServicesByDistrictReport()
        {
            return _districtService.GetServicesByDistrictReport();
        }
    }
}