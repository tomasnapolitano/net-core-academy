using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Service;
using Services.Interfaces;

namespace AcademyGestionGeneral.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        // GET: api/Service
        /// <summary>
        /// Obtiene todos los servicios
        /// </summary>
        /// <returns>Lista de Servicios</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet]
        public List<ServiceDTO> GetServices()
        {
            return _serviceService.GetServices();
        }

        // GET: api/Service/{id}
        /// <summary>
        /// Obtiene un servicio específico
        /// </summary>
        /// <returns>Servicio</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("{id}")]
        public ServiceDTO GetServiceById(int id)
        {
            return _serviceService.GetServiceById(id);
        }

        // GET: api/ServiceTypes
        /// <summary>
        /// Obtiene todos los Tipos de Servicios
        /// </summary>
        /// <returns>Lista de Tipos de Servicios</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet]
        [Route("ServiceTypes")]
        public List<ServiceTypeDTO> GetServiceTypes()
        {
            return _serviceService.GetServiceTypes();
        }

        // GET: api/ServiceTypes/{id}
        /// <summary>
        /// Obtiene un Tipo de Servicio específico
        /// </summary>
        /// <returns>Tipo de Servicio</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet]
        [Route("ServiceTypes/{id}")]
        public ServiceTypeDTO GetServiceTypeById(int id)
        {
            return _serviceService.GetServiceTypeById(id);
        }

        // POST: api/Service
        /// <summary>
        /// Inserta un Servicio nuevo.
        /// </summary>
        /// <returns>Servicio isertado</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPost]
        public ServiceDTO PostService(ServiceCreationDTO serviceCreationDTO)
        {
            return _serviceService.PostService(serviceCreationDTO);
        }

        // PUT: api/Service/{id}
        /// <summary>
        /// Actualiza los datos de un Servicio
        /// </summary>
        /// <returns>Servicio actualizado</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPut]
        public ServiceDTO UpdateService(ServiceUpdateDTO serviceUpdateDTO)
        {
            return _serviceService.UpdateService(serviceUpdateDTO);
        }

        // DELETE: api/Service/{id}
        /// <summary>
        /// Elimina un Servicio
        /// </summary>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpDelete("{id}")]
        public bool DeleteService(int id)
        {
            return _serviceService.DeleteService(id);
        }

        // PUT: api/Service/active/1
        /// <summary>
        /// Habilitar un servicio
        /// </summary>
        /// <returns>ServiceDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPut("active/{id}")]
        public ServiceDTO ActiveService(int id)
        {
            return _serviceService.ActiveService(id);
        }
    }
}