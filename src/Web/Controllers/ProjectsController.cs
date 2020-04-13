using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jineo.Models;
using Jineo.ViewModels;
using AutoMapper;
using Jineo.Data;
using Jineo.DTOs;

namespace Jineo.Controllers
{
    public class ProjectsController : Controller
    {
        readonly IMapper mapper;
        readonly ApplicationDbContext ctx;
        public ProjectsController(IMapper _mapper, ApplicationDbContext _ctx)
        {
            mapper = _mapper;
            ctx = _ctx;
            ctx.Database.EnsureCreated();
        }
    
        public IActionResult Projects()
        {
            
            var model = new ProjectsListPageViewModel() { Message = "hello"};
            model.Projects = mapper.Map<List<ProjectDTO>>(ctx.Projects);
            model.Message = ctx.Projects.Count().ToString();
            return View(model);
        }


    }
}