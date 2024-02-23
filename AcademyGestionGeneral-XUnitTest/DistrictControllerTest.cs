using AcademyGestionGeneral.Controllers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.District;
using Models.DTOs.Service;
using Models.Entities;
using Repositories;
using Repositories.Utils;
using Services;
using Utils.Middleware;

namespace AcademyGestionGeneral_XUnitTest
{
    public class DistrictControllerTest
    {
        private ManagementServiceContext _managementContextFake;
        private DistrictController _districtController;

        public DistrictControllerTest()
        {
            var options = new DbContextOptionsBuilder<ManagementServiceContext>()
            .UseInMemoryDatabase(databaseName: $"ManagementDBMemory-{Guid.NewGuid()}")
            .Options;

            _managementContextFake = new ManagementServiceContext(options);
            GenerateDB();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapConfig.CreateMapper();

            var districtRepository = new DistrictRepository(_managementContextFake, mapper);
            var districtService = new DistrictService(districtRepository);
            _districtController = new DistrictController(districtService);
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
                new Location() { LocationId = 1 , LocationName = "Ramos mejia" , PostalCode = "B1704"  , DistrictId = 1},
                new Location() { LocationId = 2 , LocationName = "3 de febrero" , PostalCode = "B1905"  , DistrictId = 2}
            };

            var district = new List<District>()
            {
                new District() { DistrictId = 1  , DistrictName = "Matanza" , AgentId = null},
                new District() { DistrictId = 2  , DistrictName = "San Martin" , AgentId = 2}
            };

            _managementContextFake.Users.AddRange(users);
            _managementContextFake.UserRoles.AddRange(role);
            _managementContextFake.Addresses.AddRange(address);
            _managementContextFake.Locations.AddRange(location);
            _managementContextFake.Districts.AddRange(district);
            _managementContextFake.SaveChanges();
        }

        /// <summary>
        /// Este metodo prueba listado de Distritos
        /// </summary>
        [Fact]
        public void GetDistricts_GetList_OkResult()
        {
            //Act
            var result = _districtController.GetDistricts();         

            //Assert            
            Assert.Equal(this._managementContextFake.Districts.ToList().Count, result.Count);
        }

        /// <summary>
        /// Este metodo prueba listado vacio de Distritos
        /// </summary>
        /// <remarks>
        /// se crea un contexto sin conexion a la base de datos ;
        /// </remarks>
        [Fact]
        public void GetDistricts_GetList_EmptyResult()
        {
            //Arrange
            var errorMessageExpected = "La lista de distritos está vacía.";
            var errorCodeExpected = System.Net.HttpStatusCode.NotFound;

            //Arrange
            var options2 = new DbContextOptionsBuilder<ManagementServiceContext>()
           .UseInMemoryDatabase(databaseName: $"InventoryDBMemory-{Guid.NewGuid()}")
            .Options;

            _managementContextFake = new ManagementServiceContext(options2);
            //GenerateDB();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapConfig.CreateMapper();

            var districtRepository = new DistrictRepository(_managementContextFake, mapper);
            var districtService = new DistrictService(districtRepository);
            _districtController = new DistrictController(districtService);

            //Act
            var exception = Assert.Throws<AggregateException>(() => _districtController.GetDistricts());
            var keyNotFoundException = exception.InnerException is KeyNotFoundException ? exception.InnerException as KeyNotFoundException : null;

            //Assert
            Assert.Equal(errorMessageExpected, keyNotFoundException?.Message);
        }

        /// <summary>
        /// Este metodo prueba buscar un distrito por id
        /// </summary>
        [Fact]
        public void GetByDistrictId()
        {
            //Arrange
            var id = 1;

            //Act
            var result = _districtController.GetDistrictById(id);

            //Assert           
            Assert.Equal(id , result.DistrictId);
            Assert.False(result == null);
        }

        /// <summary>
        /// Este metodo prueba buscar id no existente de District
        /// </summary>
        [Fact]
        public void GetByIdDistrict_ReturnsInCorrectDistrict()
        {
            //Arrange
            var districtId = 1995;
            var errorMessageExpected = "No se encontró el distrito.";
            var errorCodeExpected = System.Net.HttpStatusCode.NotFound;

            //Act
            var exception = Assert.Throws<AggregateException>(() => _districtController.GetDistrictById(districtId));
            var keyNotFoundException = exception.InnerException is KeyNotFoundException ? exception.InnerException as KeyNotFoundException : null;

            //Assert
            Assert.Equal(errorMessageExpected, keyNotFoundException?.Message);
        }

        /// <summary>
        /// Obtiene un distrito con su respectivo agente
        /// </summary>
        [Fact]
        public void GetListDistrictsWithAgents_ReturnsOk()
        {
            //Arrange
            var districtId = 2;

            //Act
            var result = _districtController.GetDistrictsWithAgent(districtId);

            //Assert           
            Assert.Equal(districtId, result.DistrictId);
            Assert.False(result == null);
        }

        /// <summary>
        /// Este metodo prueba buscar id de distrito sin agente existente
        /// </summary>
        [Fact]
        public void GetListDistrictsWithAgents_ReturnsInCorrectDistrict()
        {
            //Arrange
            var districtId = 1;
            var errorMessageExpected = "El distrito no posee agente/s a cargo.";
            var errorCodeExpected = System.Net.HttpStatusCode.BadRequest;

            //Act
            var exception = Assert.Throws<AggregateException>(() => _districtController.GetDistrictsWithAgent(districtId));
            var badRequestException = exception.InnerException is BadRequestException ? exception.InnerException as BadRequestException : null;

            //Assert
            Assert.Equal(errorMessageExpected, badRequestException?.Message);
        }

        /// <summary>
        /// Este metodo prueba asignar un agente a un distrito sin agente
        /// </summary>
        [Fact]
        public void GetAddAgentToDistrict_ReturnsOk()
        {
            // Arrange
            var agentId = 2;
            var districtId = 1;

            // Act
            var result = _districtController.AddAgentToDistrict(agentId, districtId);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Este metodo prueba asignar un usuario el cual no posee rol de agente
        /// </summary>
        [Fact]
        public void GetAddAgentToDistrict_ReturnsBadRequestNotAgent()
        {
            // Arrange
            var agentId = 3;
            var districtId = 1;
            var errorMessageExpected = "El usuario no posee rol de agente.";
            var errorCodeExpected = System.Net.HttpStatusCode.BadRequest;

            // Act
            var exception = Assert.Throws<AggregateException>(() => _districtController.AddAgentToDistrict(agentId, districtId));
            var badRequestException = exception.InnerException is BadRequestException ? exception.InnerException as BadRequestException : null;

            // Assert
            Assert.Equal(errorMessageExpected, badRequestException?.Message);
        }

        /// <summary>
        /// Este metodo prueba asignar un agente a un distrito con agente asignado
        /// </summary>
        [Fact]
        public void GetAddAgentToDistrict_ReturnsBadRequestDistrictAgentExist()
        {
            // Arrange
            var agentId = 2;
            var districtId = 2;
            var errorMessageExpected = "El distrito ya tiene un agente asignado.";
            var errorCodeExpected = System.Net.HttpStatusCode.BadRequest;

            // Act
            var exception = Assert.Throws<AggregateException>(() => _districtController.AddAgentToDistrict(agentId, districtId));
            var badRequestException = exception.InnerException is BadRequestException ? exception.InnerException as BadRequestException : null;

            // Assert
            Assert.Equal(errorMessageExpected, badRequestException?.Message);
        }

        /// <summary>
        /// Este metodo prueba remover un agente de un distrito
        /// </summary>
        [Fact]
        public void GetRemoveAgentFromDistrict_ReturnsOk()
        {
            // Arrange
            var districtId = 2;

            // Act
            var result = _districtController.RemoveAgentFromDistrict(districtId);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Este metodo prueba remover un agente de un distrito sin agente
        /// </summary>
        [Fact]
        public void GetRemoveAgentFromDistrict_ReturnsBadRequestNotAgentInDistrict()
        {
            // Arrange
            var districtId = 1;
            var errorMessageExpected = "El distrito no posee un agente asignado.";
            var errorCodeExpected = System.Net.HttpStatusCode.BadRequest;

            // Act
            var exception = Assert.Throws<AggregateException>(() => _districtController.RemoveAgentFromDistrict(districtId));
            var badRequestException = exception.InnerException is BadRequestException ? exception.InnerException as BadRequestException : null;

            // Assert
            Assert.Equal(errorMessageExpected, badRequestException?.Message);
        }
    }    
}