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

            //User
            CreateMap<JineoUser, UserDTO>();

            //Company
            CreateMap<Company, CompanyDTO>();

            //Issue
            CreateMap<Issue, IssueDTO>();

            //Comments
            CreateMap<Comment, CommentDTO>();

            
        }
    }
}