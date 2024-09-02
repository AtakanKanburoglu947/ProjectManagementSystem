﻿using AutoMapper;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore
{
    public class DtoMapper : Profile
    {
        public DtoMapper() { 
            CreateMap<JobDto,Job>().ReverseMap();
            CreateMap<RoleDto,Role>().ReverseMap();
            CreateMap<UserDto,User>().ReverseMap();
            CreateMap<JobUpdateDto, Job>().ReverseMap();
            
        }
    }
}
