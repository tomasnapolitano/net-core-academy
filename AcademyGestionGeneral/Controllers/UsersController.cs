using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Bill;
using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;
using Services;
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
        /// <returns>UserWithTokenDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public UserWithTokenDTO Login(UserLoginDTO userLoginDTO)
        {
            return _usersService.Login(userLoginDTO);
        }

        // PUT: api/Users/password
        /// <summary>
        /// Cambio de contraseña del usuario
        /// </summary>
        /// <param name="userUpdatePassDTO"></param>
        /// <returns>Bool</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPut("password")]
        public bool ChangePassword(UserUpdatePasswordDTO userUpdatePassDTO)
        {
            return _usersService.ChangePassword(userUpdatePassDTO);
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

        // GET: api/Users/services
        /// <summary>
        /// Obtiene un listado de usuarios con sus servicios suscritos
        /// </summary>
        /// <returns>List<UserWithServicesDTO>()</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpGet("services")]
        public List<UserWithServicesDTO> GetUsersWithServices()
        {
            return _usersService.GetUsersWithServices();
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
        public UserWithServicesDTO GetUserWithServicesById(int userId)
        {
            return _usersService.GetUserWithServicesById(userId);
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

        // GET: api/Users/subscription/1
        /// <summary>
        ///  Obtiene la subscripción al servicio de un usuario.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns>ServiceSubscriptionWithUserDTO</returns> 
        [HttpGet("subscription/{subscriptionId}")]
        public ServiceSubscriptionWithUserDTO GetServiceSubscriptionClient(int subscriptionId)
        {
            return _usersService.GetServiceSubscriptionClient(subscriptionId);
        }

        // GET: api/Users/subscription/1/consumption
        /// <summary>
        ///  Obtiene la consumisión de un usuario sobre un servicio al que está suscrito.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns>ConsumptionDTO</returns> 
        [HttpGet("subscription/{subscriptionId}/consumption")]
        public ConsumptionDTO GetSubscriptionConsumption(int subscriptionId)
        {
            return _usersService.GetRandomSubscriptionConsumption(subscriptionId);
        }

        // POST: api/Users/bill/{userId}/generate
        /// <summary>
        /// Genera las facturas del cliente elegido
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>BillDetailDTO</returns>
        /// <response code="200">La operación fue exitosa</response>
        /// <response code="500">Internal server error</response>
        /// <response code="400">Mal ingreso de datos</response>
        [HttpPost("bill/{userId}/generate")]
        public ConsumptionBillDTO GenerateBill(int userId)
        {
            return _usersService.GenerateBill(userId);
        } // ------ api/Users/{userId}/bills/generate -----------------------------

        // GET: api/Users/bills/1
        /// <summary>
        ///  Obtiene la los datos de una factura por Id.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>ConsumptionDTO</returns> 
        [HttpGet("bills/{billId}")]
        public ConsumptionBillDTO GetBillById(int billId)
        {
            return _usersService.GetBillById(billId);
        }

        // GET: api/Users/1/bills
        /// <summary>
        ///  Obtiene todas las facturas de un usuario por id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<ConsumptionDTO></returns> 
        [HttpGet("{userId}/bills")]
        public List<ConsumptionBillDTO> GetBillsByUserId(int userId)
        {
            return _usersService.GetBillsByUserId(userId);
        }

        // GET: api/Users/bills
        /// <summary>
        ///  Obtiene todas las facturas.
        /// </summary>
        /// <returns>List<ConsumptionBillDTO></returns> 
        [HttpGet("bills")]
        public List<ConsumptionBillDTO> GetAllBills()
        {
            return _usersService.GetAllBills();
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
        public UserDTO PostUser(UserCreationDTO userCreationDTO)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return _usersService.PostUser(userCreationDTO, token);
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