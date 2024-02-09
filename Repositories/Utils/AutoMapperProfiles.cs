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
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Districts, opt => opt.MapFrom(src => src.Districts.Select(d => new DistrictDTO { DistrictId = d.DistrictId, DistrictName = d.DistrictName }).ToList()));

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Districts, opt => opt.Ignore());


            CreateMap<District, DistrictDTO>();
            CreateMap<DistrictDTO, District>();

            CreateMap<Service, ServiceDTO>();
            CreateMap<ServiceDTO, Service>(); 
            CreateMap<ServiceCreationDTO, Service>(); 
            CreateMap<ServiceUpdateDTO, Service>(); 

            CreateMap<ServiceType, ServiceTypeDTO>();
            CreateMap<ServiceTypeDTO, ServiceType>();

            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserUpdateDTO>();
        }
    }
}