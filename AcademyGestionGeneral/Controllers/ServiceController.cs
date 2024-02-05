using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Services.Interfaces;

namespace AcademyGestionGeneral.Controllers
{
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
        public List<Service> GetServices()
        {
            return _serviceService.GetServices();
        }
    }
}