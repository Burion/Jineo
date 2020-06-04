using System;
using System.Collections.Generic;
using Jineo.DTOs;

namespace Jineo.ViewModels
{
    public class ProjectPageViewModel
    {
        public ProjectDTO Project {get;set;}
        public ProductDTO[] Products {get;set;}
        
    }
}