using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jineo.Models;
using Microsoft.AspNetCore.Identity;

namespace Jineo.Controllers
{
    public class UsersController : Controller
    {
        readonly UserManager<JineoUser> userManager;
        readonly RoleManager<IdentityRole> roleManager;
        public UsersController(UserManager<JineoUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<IActionResult> SetUserToRole(string email, string role)
        {
            if(!(await roleManager.RoleExistsAsync(role)))
            {
                throw new ArgumentException("This role doesn't exist");
            }

            var user = await userManager.FindByEmailAsync(email);
            await userManager.AddToRoleAsync(user, role);
            return RedirectToAction("Users");
        }
    }
}