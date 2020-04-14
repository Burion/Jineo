using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jineo.Data;
using Jineo.Helpers;
using Jineo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Jineo
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host = CreateHostBuilder(args).Build();
           
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                Helpers.StartupHelper.EnsureDatabaseCreated<ApplicationDbContext>(serviceProvider);
                var userManager = serviceProvider.GetRequiredService<UserManager<JineoUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                DataInit.SeedData(userManager, roleManager);
                
            }
 
            host.Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
