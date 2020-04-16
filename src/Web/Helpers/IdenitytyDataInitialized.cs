using Microsoft.AspNetCore.Identity;
using Jineo.Models;
 
namespace Jineo.Helpers
{
    public static class DataInit
    {
        public static void SeedData(UserManager<JineoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
 
        private static void SeedUsers (UserManager<JineoUser> userManager)
        {
            if (userManager.FindByEmailAsync("super@localhost").Result == null)
            {
                JineoUser user = new JineoUser();
                user.UserName = "super@localhost";
                user.Email = "super@localhost";
                user.EmailConfirmed = true;
 
                IdentityResult result = userManager.CreateAsync(user, "Vlad1_").Result;
 
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "SuperAdmin").Wait();
                }
            }
 
 
            if (userManager.FindByEmailAsync("admin@localhost").Result == null)
            {
                JineoUser user = new JineoUser();
                user.UserName = "admin@localhost";
                user.Email = "admin@localhost";
                user.CompanyId = -1;
                user.EmailConfirmed = true;
 
                IdentityResult result = userManager.CreateAsync(user, "Vlad1_").Result;
 
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
 
        private static void SeedRoles (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "SuperAdmin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
 
 
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}