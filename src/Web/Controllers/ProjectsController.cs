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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jineo.Controllers
{
    public class ProjectsController : Controller
    {
        readonly IMapper mapper;
        readonly ApplicationDbContext ctx;
        readonly UserManager<JineoUser> um;
        public ProjectsController(IMapper _mapper, ApplicationDbContext _ctx, UserManager<JineoUser> um)
        {
            mapper = _mapper;
            ctx = _ctx;
            ctx.Database.EnsureCreated();
            this.um = um;
        }
    
        // public IActionResult Projects()
        // {        
        //     var model = new ProjectsListPageViewModel() { Message = "hello"};
        //     model.Projects = mapper.Map<List<ProjectDTO>>(ctx.Projects);
        //     model.Message = ctx.Projects.Count().ToString();
        //     var user = um.FindByEmailAsync(User.Identity.Name).Result;
        //     Console.WriteLine($"USER ROLES: {um.GetRolesAsync(user).Result.First()}");
        //     Console.WriteLine(User.IsInRole("SuperAdmin"));
        //     return View(model);
        // }
        [Route("projectusers")]
        public async Task<JsonResult> ProjectUsers(string id)
        {
            var project = ctx.Projects.Include(p => p.UsersProjects).ThenInclude(up => up.User).Single(p => p.Id == int.Parse(id));
            var users = project.UsersProjects.Select(up => up.User);
            var userDTOs = mapper.Map<UserDTO[]>(users);
            return new JsonResult(new { Users = userDTOs });
        }

        [Route("projects/{id}")]
        public async Task<IActionResult> Project(string id)
        {
            ProjectPageViewModel model = new ProjectPageViewModel();
            var project = ctx.Projects.Single(p => p.Id == int.Parse(id));
            var projectDTO = mapper.Map<ProjectDTO>(project);
            model.Project = projectDTO;
            return View(model);
        }


        [Route("projects")]
        public async Task<JsonResult> Projects()
        {
            var _projects = ctx.Projects;
            var projects = mapper.Map<ProjectDTO[]>(_projects);
            return new JsonResult(new { projects });
        }

        [Route("projectinfo")]
        public async Task<JsonResult> Projects(string id)
        {
            var _project = ctx.Projects.Single(p => p.Id == int.Parse(id));
            var project = mapper.Map<ProjectDTO>(_project);
            return new JsonResult(new { project });
        }

    }
}