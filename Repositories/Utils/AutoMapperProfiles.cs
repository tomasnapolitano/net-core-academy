﻿using AutoMapper;
using Models.DTOs;
using Models.DTOs.District;
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
        }
    }
}