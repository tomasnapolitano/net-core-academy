using AutoMapper;
using Models.DTOs.District;
using Models.DTOs.Service;
using Models.DTOs.User;
using Models.Entities;

namespace Repositories.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<DistrictDTO, District>();
            CreateMap<District, DistrictDTO>();

            CreateMap<Service, ServiceDTO>();
            CreateMap<ServiceDTO, Service>(); 
            CreateMap<ServiceCreationDTO, Service>(); 

            CreateMap<ServiceType, ServiceTypeDTO>();
            CreateMap<ServiceTypeDTO, ServiceType>();

            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserUpdateDTO>();
        }
    }
}