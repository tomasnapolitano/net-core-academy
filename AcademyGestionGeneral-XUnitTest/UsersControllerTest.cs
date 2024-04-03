using AcademyGestionGeneral.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Login;
using Models.DTOs.User;
using Models.Entities;
using Repositories;
using Repositories.Utils;
using Repositories.Utils.PasswordHasher;
using Services;
using Sprache;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utils.Middleware;
using Microsoft.AspNetCore.Http.Abstractions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using Models.DTOs.Service;

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

            Environment.SetEnvironmentVariable("JWT_KEY", "jwtTestKeyyyyyyyyyyyyy====lksdmslkdmflksmdlksmd");
            Environment.SetEnvironmentVariable("JWT_ISSUER", "https://localhost:7105/");
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "https://localhost:7105/");
            Environment.SetEnvironmentVariable("JWT_SUBJECT", "baseWebApiSubject");

            var passwordHasher = new PasswordHasher();
            var usersRepository = new UsersRepository(_managementContextFake, mapper, passwordHasher);
            var usersService = new UsersService(usersRepository);
            _usersController = new UsersController(usersService);
        }

        private void GenerateDB()
        {
            var users = new List<User>
            {
                // Password para UserId=1 es "Password0":
                new User() { UserId = 1 , FirstName = "Fernando" , LastName = "Alarcon" , Email = "alarcon@test.com" , Password = "1usY83lgTX/5VK98K5Kodw==;UEsD0yHxWXFH5ZbcZuBMGoUVPzo3Wpgil7xWx+kOqmU=" , Dninumber = "15151515" , CreationDate = DateTime.Now , AddressId = 1 , RoleId = 1, Active = true },
                new User() { UserId = 2 , FirstName = "Ema" , LastName = "Roffo" , Email = "roffo@test.com" , Password = "TEST19239" , Dninumber = "19181717" , CreationDate = DateTime.Now , AddressId = 2 , RoleId = 2, Active = true  },
                new User() { UserId = 3 , FirstName = "Silvio" , LastName = "Romero" , Email = "romero@test.com" , Password = "TEST19239" , Dninumber = "42599687" , CreationDate = DateTime.Now , AddressId = 3 , RoleId = 3, Active = true  },
                new User() { UserId = 4 , FirstName = "Mario" , LastName = "Rodriguez" , Email = "rodri@test.com" , Password = "Asdff12" , Dninumber = "43121545" , CreationDate = DateTime.Now , AddressId = 3 , RoleId = 3, Active = false  }
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
                new District() { DistrictId = 1  , DistrictName = "Matanza" , AgentId = 2} 
            };

            var serviceTypes = new List<ServiceType>()
            {
                new ServiceType() { ServiceTypeId = 1, Description = "test", ServiceTypeName = "test" }
            };

            var services = new List<Service>()
            {
                new Service() { ServiceId = 1, ServiceName = "TestService", PricePerUnit = 1000, ServiceTypeId = 1, Active = true }
            };

            var districtXservices = new List<DistrictXservice>()
            {
                new DistrictXservice() { DistrictXserviceId = 1, DistrictId = 1, ServiceId = 1, Active = true }
            };

            var subscriptions = new List<ServiceSubscription>()
            {
                new ServiceSubscription() { SubscriptionId = 1, DistrictXserviceId = 1, UserId = 1, PauseSubscription = false, StartDate = DateTime.Now }
            };

            _managementContextFake.Users.AddRange(users);
            _managementContextFake.UserRoles.AddRange(role);
            _managementContextFake.Addresses.AddRange(address);
            _managementContextFake.Locations.AddRange(location);
            _managementContextFake.Districts.AddRange(district);
            _managementContextFake.DistrictXservices.AddRange(districtXservices);
            _managementContextFake.ServiceTypes.AddRange(serviceTypes);
            _managementContextFake.Services.AddRange(services);
            _managementContextFake.ServiceSubscriptions.AddRange(subscriptions);
            _managementContextFake.SaveChanges();
        }

        private string GetToken(User user)
        {
            var jwt = new Jwt
            {
                Key = Environment.GetEnvironmentVariable("JWT_KEY"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                Subject = Environment.GetEnvironmentVariable("JWT_SUBJECT")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30), // token expira en 30 minutos
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwt.Issuer,
                Audience = jwt.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Este metodo prueba login de Usuarios
        /// </summary>
        [Fact]
        public void Login_ReturnOk()
        {
            //Arrange
            UserLoginDTO userLoginDTO = new()
            {
                Email = "alarcon@test.com",
                Password = "Password0"
            };

            //Act
            var result = _usersController.Login(userLoginDTO);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<UserWithTokenDTO>(result);
        }

        /// <summary>
        /// Este metodo prueba login de Usuarios, ingresando email no existente.
        /// </summary>
        [Fact]
        public void Login_ReturnErrorNonExistingEmail()
        {
            //Arrange
            UserLoginDTO userLoginDTO = new()
            {
                Email = "fernando_uvedoble@test.com",
                Password = "Password0"
            };

            var errorMessageExpected = "El email ingresado no tiene una cuenta asociada. Por favor comuníquese con su Agente asignado para dar de alta su cuenta.";

            //Act & Assert
            var keyNotFoundException = Assert.Throws<AggregateException>(() => _usersController.Login(userLoginDTO));
            var innerExc = keyNotFoundException.InnerException;
            //Assert
            Assert.Equal(errorMessageExpected, innerExc.Message);
        }

        /// <summary>
        /// Este metodo prueba login de Usuarios, ingresando una contraseña incorrecta.
        /// </summary>
        [Fact]
        public void Login_ReturnErrorIncorrectPassword()
        {
            //Arrange
            UserLoginDTO userLoginDTO = new()
            {
                Email = "alarcon@test.com",
                Password = "Contraseñita"
            };

            var errorMessageExpected = "La contraseña ingresada no es correcta.";

            //Act & Assert
            var keyNotFoundException = Assert.Throws<AggregateException>(() => _usersController.Login(userLoginDTO));
            var innerExc = keyNotFoundException.InnerException;
            //Assert
            Assert.Equal(errorMessageExpected, innerExc.Message);
        }

        /// <summary>
        /// Este metodo prueba listado de Usuarios
        /// </summary>
        [Fact]
        public void GetUsers_OkResult()
        {
            //Act
            var result = _usersController.GetUsers();         

            //Assert            
            Assert.Equal(this._managementContextFake.Users.ToList().Count, result.Count);
        }

        /// <summary>
        /// Este metodo prueba listado de Usuarios Activos
        /// </summary>
        [Fact]
        public void GetActiveUsers_OkResult()
        {
            //Act
            var result = _usersController.GetActiveUsers();

            //Assert            
            Assert.Equal(this._managementContextFake.Users.Where(u => u.Active==true).ToList().Count, result.Count);
        }

        /// <summary>
        /// Este metodo prueba listado de Usuarios Agentes
        /// </summary>
        [Fact]
        public void GetAllAgents_OkResult()
        {
            //Act
            var result = _usersController.GetAllAgents();

            //Assert            
            Assert.Equal(this._managementContextFake.Users.Where(u => u.RoleId == 2).ToList().Count, result.Count);
        }

        /// <summary>
        /// Este metodo prueba listado de Usuarios por Distrito
        /// </summary>
        [Fact]
        public void GetUsersByDistrictId_OkResult()
        {
            //Arrange
            int districtId = 1;

            //Act
            var result = _usersController.GetUsersByDistrictId(districtId);

            //Assert            
            Assert.Equal(this._managementContextFake.Users.Include(u => u.Address).ThenInclude(a => a.Location).ThenInclude(l => l.District).Where(x => x.Address.Location.DistrictId == districtId).ToList().Count, result.Count);
        }

        /// <summary>
        /// Este metodo prueba buscar un agente con su distrito por id
        /// </summary>
        [Fact]
        public void GetAgentWithDistrtict_ReturnOk()
        {
            //Arrange
            var id = 2;

            //Act
            var result = _usersController.GetAgentsWithDistrict(id);

            //Assert           
            Assert.Equal(id, result.UserId);
            Assert.NotNull(result.Districts);
            Assert.False(result == null);
        }

        /// <summary>
        /// Este metodo prueba traer un usuario con sus servicios
        /// </summary>
        [Fact]
        public void GetUserWithServices_ReturnOk()
        {
            //Arrange
            var id = 1;

            //Act
            var result = _usersController.GetUserWithServices(id);

            //Assert           
            Assert.Equal(id, result.UserId);
            Assert.NotNull(result.ServiceSubscriptions);
            Assert.False(result == null);
        }

        /// <summary>
        /// Este metodo prueba traer un usuario con sus servicios
        /// </summary>
        [Fact]
        public void SubscribeUserToService_ReturnOk()
        {
            //Arrange
            var userId = 1;
            var serviceId = 1;

            //Act
            var result = _usersController.SubscribeUserToService(userId,serviceId);

            //Assert
            Assert.IsAssignableFrom<UserWithServicesDTO>(result);
            Assert.NotNull(_managementContextFake.ServiceSubscriptions);

        }
        
        /// <summary>
        /// Este metodo prueba traer la consumisión de una suscripción
        /// </summary>
        [Fact]
        public void GetSubscriptionConsumption_ReturnOk()
        {
            //Arrange
            var id = 1;

            //Act
            var result = _usersController.GetSubscriptionConsumption(id);

            //Assert
            Assert.IsAssignableFrom<ConsumptionDTO>(result);
        }

        /// <summary>
        /// Este metodo prueba pausar la suscripción a un servicio
        /// </summary>
        [Fact]
        public void PauseSubscribeUserToService_ReturnOk()
        {
            //Arrange
            var id = 1;

            //Act
            var result = _usersController.PauseSubscribeUserToService(id);

            //Assert
            Assert.IsAssignableFrom<UserWithServicesDTO>(result);
            Assert.Equal(result.ServiceSubscriptions.Where(s => s.SubscriptionId == id).First().PauseSubscription, true);
        }

        /// <summary>
        /// Este metodo prueba buscar un usuario por id
        /// </summary>
        [Fact]
        public void GetByUserId_ReturnOk()
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
        /// Este metodo prueba buscar un usuario por id
        /// </summary>
        [Fact]
        public void GetByUserId_ErrorNotFound()
        {
            //Arrange
            var id = 99;
            string expectedError = "No se encontró el usuario.";

            //Act
            var aggregateException = Assert.Throws<AggregateException>(() => _usersController.GetUserById(id));
            var innerException = aggregateException.InnerException;

            //Assert
            Assert.Equal(expectedError, innerException?.Message);
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
                Dninumber = "42595687",
                Neighborhood = "Test",
                Password = "Test1234",
                PostalCode = "B1704",
                DistrictId = 1,
                Email = "test@test.com",
                RoleId = 3,
                StreetNumber = "Test",
                StreetName = "Test",
            };

            string token = GetToken(_managementContextFake.Users.Find(1));

            // Simulating Token header:
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(r => r.Headers["Authorization"]).Returns($"Bearer {token}");
            mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);
            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
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
                Dninumber = "42599689",
                Neighborhood = "Test",
                Password = "Test1234",
                PostalCode = "B1704",
                DistrictId = 1,
                Email = "test@test.com",
                RoleId = 3,
                StreetNumber = "Test",
                StreetName = "Test",
            };

            var errorMessageExpected = "El usuario no tiene permisos para acceder a este recurso.";

            string token = GetToken(_managementContextFake.Users.Find(3));

            // Simulating Token header:
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(r => r.Headers["Authorization"]).Returns($"Bearer {token}");
            mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);
            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            //Act
            var aggregateException = Assert.Throws<AggregateException>(() => _usersController.PostUser(userId, newUser));
            var innerException = aggregateException.InnerException;

            //Assert
            Assert.Equal(errorMessageExpected, innerException?.Message);
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
                Password = "Test1234",
                PostalCode = "B1704",
                DistrictId = 1,
                Email = "test@test.com",
                RoleId = 3,
                StreetNumber = "Test",
                StreetName = "Test",
            };

            var errorMessageExpected = $"El DNI ingresado ya existe, no puede crear un usuario con DNI: {newUser.Dninumber}";

            string token = GetToken(_managementContextFake.Users.Find(1));

            // Simulating Token header:
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(r => r.Headers["Authorization"]).Returns($"Bearer {token}");
            mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);
            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

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
                Password = "Test1234",
                PostalCode = "AAA89",
                DistrictId = 1,
                Email = "test@test.com",
                RoleId = 3,
                StreetNumber = "Test",
                StreetName = "Test",
            };

            var errorMessageExpected = $"No se encontró localidad con el codigo postal: {newUser.PostalCode}";

            string token = GetToken(_managementContextFake.Users.Find(1));

            // Simulating Token header:
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(r => r.Headers["Authorization"]).Returns($"Bearer {token}");
            mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);
            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

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
        public void UpdateUser_ReturnsOk()
        {
            //Arrange
            var updateUser = new UserUpdateDTO()
            {
                UserId = 3,
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.com",
            };

            string token = GetToken(_managementContextFake.Users.Find(1));

            // Simulating Token header:
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(r => r.Headers["Authorization"]).Returns($"Bearer {token}");
            mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);
            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            //Act
            var result = _usersController.UpdateUser(updateUser);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<UserDTO>(result);
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

            string token = GetToken(_managementContextFake.Users.Find(1));

            // Simulating Token header:
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(r => r.Headers["Authorization"]).Returns($"Bearer {token}");
            mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);
            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            //Act
            var aggregateException = Assert.Throws<AggregateException>(() => _usersController.UpdateUser(updateUser));
            var innerException = aggregateException.InnerException;

            //Assert
            Assert.Equal(errorMessageExpected, innerException?.Message);
        }

        /// <summary>
        /// Este método prueba el borrado de un usuario.
        /// </summary> 
        [Fact]
        public async Task DeleteUser_ReturnOk()
        {
            //Arrange
            int id = 1;
            string token = GetToken(_managementContextFake.Users.Find(1));

            // Simulating Token header:
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(r => r.Headers["Authorization"]).Returns($"Bearer {token}");
            mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);
            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            //Act
            var actionResult = _usersController.DeleteUser(id);

            //Assert
            Assert.IsAssignableFrom<UserDTO>(actionResult);
            Assert.Equal(_managementContextFake.Users.Where(u => u.UserId == id).FirstOrDefault().Active, false);
        }
    }    
}