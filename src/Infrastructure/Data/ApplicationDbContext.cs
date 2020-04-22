﻿using System;
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
        public DbSet<UserProject> UsersProjects {get;set;} 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserProject>().HasKey(up => new { up.ProjectId, up.JineoUserId });
            builder.Entity<UserProject>().HasOne(up => up.Project).WithMany(p => p.UsersProjects).HasForeignKey(up => up.ProjectId);
            builder.Entity<UserProject>().HasData(
                new UserProject()
                {
                    JineoUserId = "1",
                    ProjectId = -1 
                },
                new UserProject()
                {
                    JineoUserId = "2",
                    ProjectId = -1 
                },
                new UserProject()
                {
                    JineoUserId = "1",
                    ProjectId = -2 
                }
            );
                
            builder.Entity<Project>().HasData(
                new Project() { Id = -1,  Name = "Empire State Building"}, 
                new Project() { Id = -2,  Name = "Fuck"}
                
            );

            builder.Entity<Project>().HasData(
                new Project() { Id = -3,  Name = "Empire State Building"}
            );

            builder.Entity<Company>().HasData(
                new Company() { Id = -1,  Name = "ArchitectureCompany" },
                new Company() { Id = -2, Name = "Intetics"}
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
