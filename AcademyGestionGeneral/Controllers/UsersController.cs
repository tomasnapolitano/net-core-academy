using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;
using Services.Interfaces;

namespace AcademyGestionGeneral.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // POST: api/Users/login
        /// <summary>
        /// Login a la plataforma
        /// </summary>
        /// <returns>Bool</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public UserWithTokenDTO Login(UserLoginDTO userLoginDTO)
        {
            return _usersService.Login(userLoginDTO);
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

        // GET: api/ActiveUsers
        /// <summary>
        /// Obtiene todos los usuarios activos
        /// </summary>
        /// <returns>Lista de UserDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("ActiveUsers")]
        public List<UserDTO> GetActiveUsers()
        {
            return _usersService.GetActiveUsers();
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

        // GET: api/Users/{userId}/services
        /// <summary>
        /// Obtiene un usuario con sus servicios suscritos
        /// </summary>
        /// <param name="userId">Es el id del usuario</param>
        /// <returns>UserWithServicesDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("{userId}/services")]
        public UserWithServicesDTO GetUserWithServices(int userId)
        {
            return _usersService.GetUserWithServices(userId);
        }

        // POST: api/Users/1/services/subscribe
        /// <summary>
        ///  Suscribe un usuario a un servicio específico.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="serviceId"></param>
        /// <returns>UserWithServicesDTO</returns>
        [HttpPost("{userId}/services/subscribe")]
        public UserWithServicesDTO SubscribeUserToService(int userId, [FromBody]int serviceId)
        {
            return _usersService.SubscribeUserToService(userId, serviceId);
        }

        // GET: api/Users/1/services/1/consumption
        /// <summary>
        ///  Obtiene la consumisión de un usuario sobre un servicio al que está suscrito.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns>ConsumptionDTO</returns> 
        [HttpGet("api/Users/subscription/{subscriptionId}")]
        public ConsumptionDTO GetSubscriptionConsumption(int subscriptionId)
        {
            return _usersService.GetRandomSubscriptionConsumption(subscriptionId);
        }

        // PUT: api/Users/services/1/pausesubscribe
        /// <summary>
        /// Pausa la suscripción de un usuario a un servicio específico.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns>UserWithServicesDTO</returns>
        [HttpPut("services/{subscriptionId}/pausesubscribe")]
        public UserWithServicesDTO PauseSubscribeUserToService(int subscriptionId)
        {
            return _usersService.PauseSubscribeUserToService(subscriptionId);
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
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return _usersService.PostUser(userId , userCreationDTO, token);
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
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return _usersService.UpdateUser(userUpdateDTO, token);
        }

        // DELETE: api/Users/1
        /// <summary>
        /// Dar de baja un usuario
        /// </summary>
        /// <returns>UserDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpDelete("{id}")]
        public UserDTO DeleteUser(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return _usersService.DeleteUser(id, token);
        }
    }
}