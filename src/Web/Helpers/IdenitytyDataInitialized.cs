using System;
using Jineo.Models;
using Microsoft.AspNetCore.Identity;

namespace Jineo.Helpers
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(UserManager<JineoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<JineoUser> userManager)
        {
            RegisterUser(userManager, "SuperAdm", "superadmin@gmail.com", "qweasd", "SuperAdmin");
            RegisterUser(userManager, "Owner", "owner@gmail.com", "qweasd", "Admin");
            RegisterUser(userManager, "OrdinaryGuy", "guy@gmail.com", "qweasd");

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            RegisterRole(roleManager, "Admin");
            RegisterRole(roleManager, "SuperAdmin");
        }

        public static JineoUser RegisterUser(UserManager<JineoUser> userManager, string userName, string email, string password)
        {
            if (userManager.FindByNameAsync(userName).Result == null)
            {
                JineoUser user = new JineoUser();
                user.UserName = userName;
                user.Email = email;

                IdentityResult result = userManager.CreateAsync(user, password).Result;
                return user;
            }
            throw new Exception();
        }
        public static JineoUser RegisterUser(UserManager<JineoUser> userManager, string userName, string email, string password, string role)
        {
            var user = RegisterUser(userManager, userName, email, "qweasd");
            userManager.AddToRoleAsync(user, role).Wait();
            return user;
        }
        public static bool RegisterRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExistsAsync(roleName).Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                return roleResult.Succeeded;
            }
            return false;
        }

    }
}