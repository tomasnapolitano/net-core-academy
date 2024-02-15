using AcademyGestionGeneral.Controllers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Service;
using Models.Entities;
using Repositories;
using Repositories.Utils;
using Services;

namespace AcademyGestionGeneral_XUnitTest
{
    public class ServiceControllerTest
    {
        private ManagementServiceContext _managementContextFake;
        private ServiceController _serviceController;

        public ServiceControllerTest()
        {
            var options = new DbContextOptionsBuilder<ManagementServiceContext>()
            .UseInMemoryDatabase(databaseName: $"ManagementDBMemory-{Guid.NewGuid()}")
            .Options;

            _managementContextFake = new ManagementServiceContext(options);
            GenerateDB();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapConfig.CreateMapper();

            var serviceRepository = new ServiceRepository(_managementContextFake, mapper);
            var serviceService = new ServiceService(serviceRepository);
            _serviceController = new ServiceController(serviceService);
        }

        private void GenerateDB()
        {
            var services = new List<Service>
            {
                new Service() { ServiceId = 1 , ServiceName = "Servicio 1" , PricePerUnit = 5400 , ServiceTypeId = 1},
                new Service() { ServiceId = 2 , ServiceName = "Servicio 2" , PricePerUnit = 100 , ServiceTypeId = 2},
                new Service() { ServiceId = 3 , ServiceName = "Servicio 3" , PricePerUnit = 10000, ServiceTypeId = 3},
            };

            var serviceTypes = new List<ServiceType>
            {
                new ServiceType() { ServiceTypeId = 1  , ServiceTypeName = "Agua" , Description = "Agua Description" },
                new ServiceType() { ServiceTypeId = 2  , ServiceTypeName = "Electricidad" , Description = "Electricidad Description" },
                new ServiceType() { ServiceTypeId = 3  , ServiceTypeName = "Gas" , Description = "Gas Description" },

            };

            _managementContextFake.Services.AddRange(services);
            _managementContextFake.ServiceTypes.AddRange(serviceTypes);
            _managementContextFake.SaveChanges();
        }

        /// <summary>
        /// Este metodo prueba el get de listado de Servicios
        /// </summary>
        [Fact]
        public void ServiceController_GetServices_ReturnOK()
        {
            //Act
            var result = _serviceController.GetServices();

            //Assert            
            Assert.Equal(this._managementContextFake.Services.ToList().Count, result.Count);
        }

        /// <summary>
        /// Este método prueba el get de listado vacio de Servicios
        /// </summary>
        [Fact]
        public void ServiceController_GetServices_EmptyResult()
        {
            _managementContextFake.Services.RemoveRange(_managementContextFake.Services);
            _managementContextFake.SaveChanges();
            var errorMessageExpected = "La lista de servicios está vacía.";
            var errorCodeExpected = System.Net.HttpStatusCode.NotFound;

            //Act
            var exception = Assert.Throws<AggregateException>(() => _serviceController.GetServices());
            var keyNotFoundException = exception.InnerException is KeyNotFoundException ? exception.InnerException as KeyNotFoundException : null;

            //Assert
            Assert.Equal(errorMessageExpected, keyNotFoundException?.Message);
        }

        /// <summary>
        /// Este metodo prueba el get de un servicio
        /// </summary> 
        [Theory]
        [InlineData(1)]
        public void ServiceController_GetServiceById_ReturnOK(int id)
        {
            //Act
            var result = _serviceController.GetServiceById(id);
            //Assert
            Assert.Equal(id, result.ServiceId);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Este método prueba el get de Servicios por id inexistente
        /// </summary>
        [Fact]
        public void ServiceController_GetServiceById_NotFound()
        {
            _managementContextFake.Services.RemoveRange(_managementContextFake.Services);
            _managementContextFake.SaveChanges();
            int idToSearch = 1;
            var errorMessageExpected = $"No se encontró servicio con el Id: {idToSearch}";
            var errorCodeExpected = System.Net.HttpStatusCode.NotFound;

            //Act
            var exception = Assert.Throws<AggregateException>(() => _serviceController.GetServiceById(idToSearch));
            var keyNotFoundException = exception.InnerException is KeyNotFoundException ? exception.InnerException as KeyNotFoundException : null;

            //Assert
            Assert.Equal(errorMessageExpected, keyNotFoundException?.Message);
        }

        /// <summary>
        /// Este metodo prueba el get de listado de tipos de servicio
        /// </summary> 
        [Fact]
        public void ServiceController_GetServiceTypes_ReturnOK()
        {
            //Act
            var result = _serviceController.GetServiceTypes();

            //Assert            
            Assert.Equal(this._managementContextFake.ServiceTypes.ToList().Count, result.Count);
        }

        /// <summary>
        /// Este método prueba el get de listado vacio de Tipos de Servicio
        /// </summary>
        /// Se crea un nuevo context ya que la relación entre tablas Service y ServiceType lo requiere
        [Fact]
        public void ServiceController_GetServiceTypes_EmptyResult()
        {
            var options2 = new DbContextOptionsBuilder<ManagementServiceContext>()
           .UseInMemoryDatabase(databaseName: $"InventoryDBMemory-{Guid.NewGuid()}")
            .Options;

            _managementContextFake = new ManagementServiceContext(options2);

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapConfig.CreateMapper();

            var serviceRepository = new ServiceRepository(_managementContextFake, mapper);
            var serviceService = new ServiceService(serviceRepository);
            _serviceController = new ServiceController(serviceService);
            var errorMessageExpected = "La lista de tipos de servicios está vacía.";
            var errorCodeExpected = System.Net.HttpStatusCode.NotFound;

            //Act
            var exception = Assert.Throws<AggregateException>(() => _serviceController.GetServiceTypes());
            var keyNotFoundException = exception.InnerException is KeyNotFoundException ? exception.InnerException as KeyNotFoundException : null;

            //Assert
            Assert.Equal(errorMessageExpected, keyNotFoundException?.Message);
        }

        /// <summary>
        /// Este metodo prueba el get de un tipo de servicio
        /// </summary> 
        [Theory]
        [InlineData(1)]
        public void ServiceController_GetServiceTypeById_ReturnOK(int id)
        {
            //Act
            var result = _serviceController.GetServiceTypeById(id);

            //Assert
            Assert.Equal(id, result.ServiceTypeId);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Este método prueba el get de Tipo de Servicio por id inexistente
        /// </summary>
        [Fact]
        public void ServiceController_GetServiceTypeById_NotFound()
        {
            int idToSearch = 99999;
            var errorMessageExpected = $"No se encontró tipo de servicio con el Id: {idToSearch}";
            var errorCodeExpected = System.Net.HttpStatusCode.NotFound;

            //Act
            var exception = Assert.Throws<AggregateException>(() => _serviceController.GetServiceTypeById(idToSearch));
            var keyNotFoundException = exception.InnerException is KeyNotFoundException ? exception.InnerException as KeyNotFoundException : null;

            //Assert
            Assert.Equal(errorMessageExpected, keyNotFoundException?.Message);
        }

        /// <summary>
        /// Este metodo prueba el post de un servicio
        /// </summary> 
        [Fact]
        public void ServiceController_PostService_ReturnOK()
        {
            // Arrange
            var newService = new ServiceCreationDTO()
            {
                ServiceName = "Servicio Post",
                PricePerUnit = 5000,
                ServiceTypeId = 1
            };

            // Act
            var result = _serviceController.PostService(newService);


            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ServiceDTO>(result);
        }

        /// <summary>
        /// Este metodo prueba el post de un servicio con un tipo de servicio inexistente
        /// </summary> 
        [Fact]
        public void ServiceController_PostService_ServiceTypeNotFound()
        {
            // Arrange
            var newService = new ServiceCreationDTO()
            {
                ServiceName = "Servicio Post",
                PricePerUnit = 5000,
                ServiceTypeId = 0
            };
            var errorMessageExpected = $"No se encontró tipo de servicio con el Id: {newService.ServiceTypeId}";
            var errorCodeExpected = System.Net.HttpStatusCode.NotFound;

            //Act
            var exception = Assert.Throws<AggregateException>(() => _serviceController.PostService(newService));
            var keyNotFoundException = exception.InnerException is KeyNotFoundException ? exception.InnerException as KeyNotFoundException : null;

            //Assert
            Assert.Equal(errorMessageExpected, keyNotFoundException?.Message);
        }

        /// <summary>
        /// Este metodo prueba el update de un servicio
        /// </summary> 
        [Fact]
        public void ServiceController_UpdateService_ReturnOK()
        {
            // Arrange
            var updatedService = new ServiceUpdateDTO()
            {
                ServiceId = 1,
                ServiceName = "Servicio Updated",
                PricePerUnit = 6000,
                ServiceTypeId = 2
            };

            // Act
            var result = _serviceController.UpdateService(updatedService);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ServiceDTO>(result);
            Assert.Equal(updatedService.ServiceId, result.ServiceId);
            Assert.Equal(updatedService.ServiceName, result.ServiceName);
            Assert.Equal(updatedService.PricePerUnit, result.PricePerUnit);
            Assert.Equal(updatedService.ServiceTypeId, result.ServiceTypeId);
        }

        /// <summary>
        /// Este metodo prueba el update de un servicio inexistente
        /// </summary> 
        [Fact]
        public void ServiceController_UpdateService_NotFound()
        {
            // Arrange
            var updatedService = new ServiceUpdateDTO()
            {
                ServiceId = 0,
                ServiceName = "Servicio Updated",
                PricePerUnit = 6000,
                ServiceTypeId = 2
            };

            var errorMessageExpected = $"No se encontró servicio con el Id: {updatedService.ServiceId}";
            var errorCodeExpected = System.Net.HttpStatusCode.NotFound;

            //Act
            var exception = Assert.Throws<AggregateException>(() => _serviceController.UpdateService(updatedService));
            var keyNotFoundException = exception.InnerException is KeyNotFoundException ? exception.InnerException as KeyNotFoundException : null;

            //Assert
            Assert.Equal(errorMessageExpected, keyNotFoundException?.Message);
        }

        // Comentado debido a que falta implementar baja lógica:
        //[Theory]
        //[InlineData(1)]
        //public void ServiceController_DeleteService_ReturnOK(int id)
        //{
        //    // Act
        //    _serviceController.DeleteService(id);
        //    // Assert
        //    Assert.NotNull(_managementContextFake.Services.Find(id));
        //}
    }
}
