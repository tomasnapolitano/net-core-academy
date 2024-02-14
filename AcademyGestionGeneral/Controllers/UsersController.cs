using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTOs.User;
using Services.Interfaces;

namespace AcademyGestionGeneral.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/Users
        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        /// <returns>Lista de UserDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet]
        public List<UserDTO> GetUsers()
        {
            return _usersService.GetUsers();
        }

        // GET: api/Users/UsersFullName
        /// <summary>
        /// Obtiene todos los usuarios con su nombre completo
        /// </summary>
        /// <returns>Lista de UserDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("UsersFullName")]
        public List<UserDTO> GetUsersWithFullName()
        {
            return _usersService.GetUsersWithFullName();
        }

        // GET: api/Users/5
        /// <summary>
        /// Obtiene un usuario
        /// </summary>
        /// <param name="id">Es el id del usuario</param>
        /// <returns>UsuarioDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("{id}")]
        public UserDTO GetUserById(int id)
        {
            return _usersService.GetUserById(id);
        }

        // GET: api/Users/byDistrict/5
        /// <summary>
        /// Obtiene una lista de usuarios de un distrito en especifico
        /// </summary>
        /// <param name="districtId">Es el id del distrito</param>
        /// <returns>Lista de UsuarioDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("Districts/{districtId}")]
        public List<UserDTO> GetUsersByDistrictId(int districtId)
        {
            return _usersService.GetUsersByDistrictId(districtId);
        }

        // GET: api/Users/Agents/5
        /// <summary>
        /// Obtiene un agente con sus respectivos distritos
        /// </summary>
        /// <param name="agentId">Es el id del agente</param>
        /// <returns>AgentDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("Agents/{agentId}")]
        public AgentDTO GetAgentsWithDistrict(int agentId)
        {
            return _usersService.GetAgentsWithDistrict(agentId);
        }

        /// <summary>
        ///  Crear un nuevo usuario (Agente o Cliente), dependiendo el usuario recibido por argumento.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userCreationDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public UserDTO PostUser(int userId , UserCreationDTO userCreationDTO)
        {
            return _usersService.PostUser(userId , userCreationDTO);
        }

        // GET: api/Users/Agents
        /// <summary>
        /// Obtiene todos los agentes del sistema
        /// </summary>
        /// <returns>Lista de UserDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("Agents")]
        public List<UserDTO> GetAllAgents()
        {
            return _usersService.GetAllAgents();
        }

        // PUT: api/Users/1
        /// <summary>
        /// Actualiza los datos de un usuario
        /// </summary>
        /// <returns>UserDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPut("{id}")]
        public UserDTO UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            return _usersService.UpdateUser(userUpdateDTO);
        }
    }
}