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
            if (userManager.FindByEmailAsync("johndoe@localhost").Result == null)
            {
                JineoUser user = new JineoUser();
                user.UserName = "johndoe@localhost";
                user.Email = "johndoe@localhost";
 
                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd1!").Result;
 
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
 
 
            if (userManager.FindByEmailAsync("alex@localhost").Result == null)
            {
                JineoUser user = new JineoUser();
                user.UserName = "alex@localhost";
                user.Email = "alex@localhost";
 
                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd1!").Result;
 
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
 
        private static void SeedRoles (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
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