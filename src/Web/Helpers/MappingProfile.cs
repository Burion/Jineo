using System;
using AutoMapper;
using Jineo.DTOs;
using Jineo.Models;

namespace Jineo.Helpers
{   
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            //Project
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>();

            
        }
    }
}