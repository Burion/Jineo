using System;
using System.Collections.Generic;
using System.Text;
using Jineo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jineo.Data
{
    public class ApplicationDbContext : IdentityDbContext<JineoUser>
    {
        public DbSet<Project> Projects {get;set;}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>().HasData(
                new Project() { Id = -1,  Name = "Empire State Building"}, 
                new Project() { Id = -2,  Name = "Fuck"}
                
            );

            builder.Entity<Project>().HasData(
                new Project() { Id = -3,  Name = "Empire State Building"}
            );
        }

        public void AddEntity<T>(T model) where T: class
        {
            //AutoMapper transform
            var set = Set<T>();
            set.Add(model);
            SaveChanges();
        }

        public async void AddEntityAsync<T>(T model) where T: class
        {
            //AutoMapper transform
            var set = Set<T>();
            await set.AddAsync(model);
            await SaveChangesAsync();
        }

        public void RemoveEntity<T>(T model) where T: class
        {
            //AutoMapper transform
            var set = Set<T>();
            set.Remove(model);
            SaveChanges();
        }
        
    }
}
