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
        public async Task<IActionResult> ChangeSubForMe(string subId)
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            user.SubscriptionId = int.Parse(subId);
            await userManager.UpdateAsync(user);
            return RedirectToAction("Subscriptions");
        }

        [Route("/changesub")]
        public async Task<IActionResult> ChangeSub(string email, string subId)
        {
            var user = await userManager.FindByEmailAsync(email);
            user.SubscriptionId = int.Parse(subId);
            await userManager.UpdateAsync(user);
            return new JsonResult(new { status = "OK"} );
        }

        [Route("/ban")]
        public async Task<IActionResult> Ban(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            user.IsBanned = !user.IsBanned;
            var res = await userManager.UpdateAsync(user);
            if(user.IsBanned)
                await userManager.AddToRoleAsync(user, "Banned");
            else
                await userManager.RemoveFromRoleAsync(user, "Banned");

            return new JsonResult(new { banned = user.IsBanned, email = user.Email });
        }

        public async Task<IActionResult> AllUsers()
        {
            AllUsersModel model = new AllUsersModel();
            var users = ctx.Users.ToArray();
            model.Users = mapper.Map<UserDTO[]>(users);
            return View(model);
        }

        public async Task<IActionResult> Subscriptions()
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            ViewBag.SubId = user.SubscriptionId;
            return View();
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

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UsersAdmin()
        {
            UsersPageViewModel model = new UsersPageViewModel();
            
            return View(model);
        }

        

        public async Task<IActionResult> AddUserToCompanySuper(string email, string comId)
        {
            var user = await userManager.FindByEmailAsync(email);
            user.CompanyId = int.Parse(comId);
            await userManager.UpdateAsync(user);
            return RedirectToAction("UsersSuperAdmin");
        }
    }
}