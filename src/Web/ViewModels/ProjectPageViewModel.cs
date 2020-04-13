using System;
using System.Collections.Generic;
using Jineo.DTOs;

namespace Jineo.ViewModels
{
    public class ProjectsListPageViewModel
    {
        public string Message { get; set; }
        public List<ProjectDTO> Projects {get; set;}
    }
}