using AcademyGestionGeneral.Controllers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repositories;
using Repositories.Utils;
using Services;

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
                /*
                new User() { UserId = 1, FirstName = "123", LastName = "ABC",
                                  Email="sfhjdhjsfh", DNINumber="31561515", Phone = "21231231" },
                new User() { UserId = 2, FirstName = "123", LastName = "ABC",
                                  Email="sfhjdhjsfh", Identification="31561515", Phone = "21231231" },
                new User() { UserId = 3, FirstName = "123", LastName = "ABC",
                                  Email="sfhjdhjsfh", Identification="31561515", Phone = "21231231" }

                */
            };

            _managementContextFake.Users.AddRange(users);
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
        /// Este metodo prueba listado vacio de Usuarios
        /// </summary>
        /// <remarks>
        /// se crea un contexto sin conexion a la base de datos ;
        /// </remarks>
        [Fact]
        public void GetUsers_GetList_EmptyResult()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ManagementServiceContext>()
           .UseInMemoryDatabase(databaseName: $"ManagementDBMemory-{Guid.NewGuid()}")
           .Options;

            _managementContextFake = new ManagementServiceContext(options);

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapConfig.CreateMapper();

            var usersRepository = new UsersRepository(_managementContextFake, mapper);
            var usersService = new UsersService(usersRepository);
            _usersController = new UsersController(usersService);

            //Act
            var result = _usersController.GetUsers();

            //Assert
            Assert.True(result.Count == 0);
        }
    }    
}