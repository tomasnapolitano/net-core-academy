using AutoMapper;
using Models.DTOs.Bill;
using Models.DTOs.District;
using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;
using Models.Entities;

namespace Repositories.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<User, UserWithTokenDTO>();

            CreateMap<User, UserWithServicesDTO>().ForMember(u => u.ServiceSubscriptions, opt => opt.Ignore());
            CreateMap<UserWithServicesDTO, UserDTO>();

            CreateMap<AgentDTO, User>();
            CreateMap<User, AgentDTO>();

            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserUpdateDTO>();

            CreateMap<ServiceSubscription,ServiceSubscriptionWithUserDTO>();
            CreateMap<ServiceSubscription,ServiceSubscriptionDTO>().ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.DistrictXservice.Service));

            CreateMap<Address,AddressDTO>();
            CreateMap<Location,LocationDTO>();

            CreateMap<District, DistrictDTO>();
            CreateMap<DistrictDTO, District>();

            CreateMap<District, DistrictAgentDTO>();
            CreateMap<DistrictAgentDTO, District>();

            CreateMap<District, DistrictWithServicesDTO>();

            CreateMap<DistrictXservice, DistrictXserviceDTO>();

            CreateMap<Service, ServiceDTO>();
            CreateMap<ServiceDTO, Service>(); 

            CreateMap<ServiceCreationDTO, Service>(); 

            CreateMap<ServiceUpdateDTO, Service>(); 

            CreateMap<ServiceType, ServiceTypeDTO>();
            CreateMap<ServiceTypeDTO, ServiceType>();

            CreateMap<BillDetail, BillDetailDTO>();
            CreateMap<BillDetailDTO, BillDetail>().ForMember(bd => bd.Subscription, opt => opt.Ignore())
                                                  .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.Subscription.SubscriptionId));
                                                  

            CreateMap<ConsumptionBill, ConsumptionBillDTO>();
            CreateMap<ConsumptionBillDTO, ConsumptionBill>();

            CreateMap<BillStatus, BillStatusDTO>();
        }
    }
}