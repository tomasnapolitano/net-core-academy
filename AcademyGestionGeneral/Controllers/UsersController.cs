using Microsoft.AspNetCore.Mvc;
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
    }
}