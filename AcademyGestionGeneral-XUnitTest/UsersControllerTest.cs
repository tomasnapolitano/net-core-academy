using AcademyGestionGeneral.Controllers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.User;
using Models.Entities;
using Repositories;
using Repositories.Utils;
using Services;
using Utils.Middleware;

namespace AcademyGestionGeneral_XUnitTest
{
    public class UsersControllerTest
    {
        private ManagementServiceContext _managementContextFake;
        private UsersController _usersController;

        public UsersControllerTest()
        {
            var options = new DbContextOptionsBuilder<ManagementServiceContext>()
            .UseInMemoryDatabase(databaseName: $"ManagementDBMemory-{Guid.NewGuid()}")
            .Options;

            _managementContextFake = new ManagementServiceContext(options);
            GenerateDB();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapConfig.CreateMapper();

            var usersRepository = new UsersRepository(_managementContextFake, mapper);
            var usersService = new UsersService(usersRepository);
            _usersController = new UsersController(usersService);
        }

        private void GenerateDB()
        {
            var users = new List<User>
            {
                new User() { UserId = 1 , FirstName = "Fernando" , LastName = "Alarcon" , Email = "alarcon@test.com" , Password = "TEST19239" , Dninumber = "15151515" , CreationDate = DateTime.Now , AddressId = 1 , RoleId = 1 },
                new User() { UserId = 2 , FirstName = "Ema" , LastName = "Roffo" , Email = "roffo@test.com" , Password = "TEST19239" , Dninumber = "19181717" , CreationDate = DateTime.Now , AddressId = 2 , RoleId = 2 },
                new User() { UserId = 3 , FirstName = "Silvio" , LastName = "Romero" , Email = "romero@test.com" , Password = "TEST19239" , Dninumber = "42599687" , CreationDate = DateTime.Now , AddressId = 3 , RoleId = 3 }
            };

            var role = new List<UserRole>
            {
                new UserRole() { RoleId = 1  , RoleName = "Admin" , RoleDescription = "Admin" },
                new UserRole() { RoleId = 2  , RoleName = "Agent" , RoleDescription = "Agente" },
                new UserRole() { RoleId = 3  , RoleName = "Client" , RoleDescription = "Cliente" },
            };

            var address = new List<Address>
            { 
                new Address() { AddressId = 1 , StreetName = "Hipolito" , StreetNumber = "1" , Neighborhood = "Santa fé" , LocationId = 1 }, 
                new Address() { AddressId = 2 , StreetName = "Hipolito 1" , StreetNumber = "2" , Neighborhood = "Barrio 1" , LocationId = 1 },
                new Address() { AddressId = 3 , StreetName = "Hipolito 3" , StreetNumber = "3" , Neighborhood = "Barrio 2" , LocationId = 1 } 
            };

            var location = new List<Location>()
            {
                new Location() { LocationId = 1 , LocationName = "Ramos mejia" , PostalCode = "B1704"  , DistrictId = 1}
            };

            var district = new List<District>()
            {
                new District() { DistrictId = 1  , DistrictName = "Matanza" , AgentId = null} 
            };

            _managementContextFake.Users.AddRange(users);
            _managementContextFake.UserRoles.AddRange(role);
            _managementContextFake.Addresses.AddRange(address);
            _managementContextFake.Locations.AddRange(location);
            _managementContextFake.Districts.AddRange(district);
            _managementContextFake.SaveChanges();
        }

        /// <summary>
        /// Este metodo prueba listado de Usuarios
        /// </summary>
        [Fact]
        public void GetUsers_GetList_OkResult()
        {
            //Act
            var result = _usersController.GetUsers();         

            //Assert            
            Assert.Equal(this._managementContextFake.Users.ToList().Count, result.Count);
        }

        /// <summary>
        /// Este metodo prueba buscar un usuario por id
        /// </summary>
        [Fact]
        public void GetByUserId()
        {
            //Arrange
            var id = 1;

            //Act
            var result = _usersController.GetUserById(id);

            //Assert           
            Assert.Equal(id , result.UserId);
            Assert.False(result == null);
        }

        /// <summary>
        /// Agregar un nuevo usuario con datos válidos, con rol de administrador
        /// </summary>
        [Fact]
        public void PostUser_ReturnsOk()
        {
            //Arrange
            var userId = 1;
            var newUser = new UserCreationDTO() 
            { 
                FirstName = "Test", 
                LastName = "Test", 
                Dninumber = "Test", 
                Neighborhood = "Test", 
                Password = "Test", 
                PostalCode = "B1704", 
                DistrictId = 1, 
                Email = "test@test.com",
                RoleId = 3,
                StreetNumber = "Test",
                StreetName = "Test",
            };

            //Act
            var result = _usersController.PostUser(userId, newUser);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<UserDTO>(result);
        }

        /// <summary>
        /// Excepcion al intentar crear un usuario, el usuario que solicita crear no tiene rol de administrador o agente.
        /// </summary>
        [Fact]
        public void PostUser_ReturnExceptionUserRoleInvalid()
        {
            //Arrange
            var userId = 3;
            var newUser = new UserCreationDTO()
            {
                FirstName = "Test",
                LastName = "Test",
                Dninumber = "Test",
                Neighborhood = "Test",
                Password = "Test",
                PostalCode = "B1704",
                DistrictId = 1,
                Email = "test@test.com",
                RoleId = 3,
                StreetNumber = "Test",
                StreetName = "Test",
            };

            var errorMessageExpected = "El id usuario enviado por parametro no puede crear usuarios. No es ADMIN/AGENTE";

            //Act
            var badRequestException = Assert.Throws<BadRequestException>(() => _usersController.PostUser(userId, newUser));
            Assert.Equal(errorMessageExpected, badRequestException.Message);

            //Assert
            Assert.Equal(errorMessageExpected, badRequestException?.Message);
        }

        /// <summary>
        /// Excepcion al intentar crear un usuario, el dni ingresado ya existe en la BD.
        /// </summary>
        [Fact]
        public void PostUser_ReturnExceptionDniExists()
        {
            //Arrange
            var userId = 1;
            var newUser = new UserCreationDTO()
            {
                FirstName = "Test",
                LastName = "Test",
                Dninumber = "42599687",
                Neighborhood = "Test",
                Password = "Test",
                PostalCode = "B1704",
                DistrictId = 1,
                Email = "test@test.com",
                RoleId = 3,
                StreetNumber = "Test",
                StreetName = "Test",
            };

            var errorMessageExpected = $"El DNI ingresado ya existe, no puede crear un usuario con DNI: {newUser.Dninumber}"; 

            //Act
            var aggregateException = Assert.Throws<AggregateException>(() => _usersController.PostUser(userId, newUser));
            var innerException = aggregateException.InnerException;

            //Assert
            Assert.Equal(errorMessageExpected, innerException?.Message);
        }

        /// <summary>
        /// Excepcion al intentar crear un usuario, el codigo postal enviado no existe.
        /// </summary>
        [Fact]
        public void PostUser_ReturnExceptionLocationNotFound()
        {
            //Arrange
            var userId = 1;
            var newUser = new UserCreationDTO()
            {
                FirstName = "Test",
                LastName = "Test",
                Dninumber = "78978997",
                Neighborhood = "Test",
                Password = "Test",
                PostalCode = "AAA89",
                DistrictId = 1,
                Email = "test@test.com",
                RoleId = 3,
                StreetNumber = "Test",
                StreetName = "Test",
            };

            var errorMessageExpected = $"No se encontró localidad con el codigo postal: {newUser.PostalCode}";

            //Act
            var aggregateException = Assert.Throws<AggregateException>(() => _usersController.PostUser(userId, newUser));
            var innerException = aggregateException.InnerException;

            //Assert
            Assert.Equal(errorMessageExpected, innerException?.Message);
        }

        /// <summary>
        /// Actualizar los datos de un usuario
        /// </summary>
        [Fact]
        public void PutUser_ReturnsOk()
        {
            //Arrange
            var updateUser = new UserUpdateDTO()
            {
                UserId = 3,
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.com",
            };

            //Act
            var result = _usersController.UpdateUser(updateUser);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<UserDTO>(result);
        }

        /// <summary>
        /// Obtener una lista de todos los agentes del sistema
        /// </summary>
        [Fact]
        public void GetListAgents_ReturnsOk()
        {
            //Act
            var result = _usersController.GetAllAgents();

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(this._managementContextFake.Users.Where(x=> x.RoleId == 2).ToList().Count, result.Count);
        }

        /// <summary>
        /// Obtener una lista de todos usuario con su nombre completo
        /// </summary>
        [Fact]
        public void GetListUserFullName_ReturnsOk()
        {
            //Act
            var result = _usersController.GetUsersWithFullName();

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(this._managementContextFake.Users.ToList().Count, result.Count);
        }

        /// <summary>
        /// Excepcion al intentar actualizar los datos de un usuario que no existe
        /// </summary>
        [Fact]
        public void PutUser_ReturnsExceptionUserId()
        {
            //Arrange
            var updateUser = new UserUpdateDTO()
            {
                UserId = 9999,
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.com",
            };

            var errorMessageExpected = $"No se encontró un usuario con el Id ingresado.";

            //Act
            var aggregateException = Assert.Throws<AggregateException>(() => _usersController.UpdateUser(updateUser));
            var innerException = aggregateException.InnerException;

            //Assert
            Assert.Equal(errorMessageExpected, innerException?.Message);
        }
    }    
}