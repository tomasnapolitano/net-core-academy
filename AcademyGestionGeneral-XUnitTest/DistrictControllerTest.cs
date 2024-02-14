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
        /// Obtener una lista de todos los agentes del sistema
        /// </summary>
        [Fact]
        public void GetListDistrictsWithAgents_ReturnsOk()
        {
            //Arrange
            var districtId = 2;

            //Act
            var result = _districtController.GetDistrictsWithAgent(districtId);

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(this._managementContextFake.Districts.ToList().Count, result.Count);
        }

        /// <summary>
        /// Obtener una lista de todos usuario con su nombre completo
        /// </summary>
        [Fact]
        public void GetListUserFullName_ReturnsOk()
        {
            //Act
            var result = _districtController.GetDistrictsWithAgent(districtId);

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(this._managementContextFake.Users.ToList().Count, result.Count);
        }
    }    
}