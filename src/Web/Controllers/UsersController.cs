using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jineo.Models;
using Microsoft.AspNetCore.Identity;
using Jineo.ViewModels;
using Jineo.Data;
using AutoMapper;
using Jineo.DTOs;
using Microsoft.AspNetCore.Authorization;
using Jineo.Extentions;

namespace Jineo.Controllers
{
    public class UsersController : Controller
    {
        readonly UserManager<JineoUser> userManager;
        readonly RoleManager<IdentityRole> roleManager;
        readonly ApplicationDbContext ctx;

        readonly IMapper mapper;
        public UsersController(UserManager<JineoUser> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _ctx, IMapper _mapper)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            ctx = _ctx;
            mapper = _mapper;
        }

        public async Task<IActionResult> SetUserToRole(string email, string role)
        {
            if(!(await roleManager.RoleExistsAsync(role)))
            {
                throw new ArgumentException("This role doesn't exist");
            }
            
            var user = await userManager.FindByEmailAsync(email);
            await userManager.RemoveFromRolesAsync(user, roleManager.Roles.Select(r => r.Name));
            await userManager.AddToRoleAsync(user, role);
            return RedirectToAction("UsersSuperAdmin");
        }

        public async Task<IActionResult> Users()
        {
            UsersPageViewModel model = new UsersPageViewModel();
            model.Users = mapper.Map<List<UserDTO>>(ctx.Users);
            return View(model);
        }

        [Authorize(Roles="SuperAdmin")]
        public async Task<IActionResult> UsersSuperAdmin()
        {
            UsersPageViewModel model = new UsersPageViewModel();
            model.Users = mapper.Map<List<UserDTO>>(ctx.Users);
            for(int x = 0; x < model.Users.Count(); x++)
            {
                model.Users[x].Role = userManager.GetRolesAsync(userManager.FindByEmailAsync(model.Users[x].Email).Result).Result.First();
            }
            return View(model);
        }
    }
}