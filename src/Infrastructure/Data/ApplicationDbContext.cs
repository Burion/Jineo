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
        public DbSet<Issue> Issues {get;set;}
        public DbSet<Comment> Comments {get;set;}
        public DbSet<UserProject> UsersProjects {get;set;} 
        public DbSet<Sensor> Sensors { get;set;} 
        public DbSet<Product> Products {get;set;}
        public DbSet<ProductLink> ProductLinks {get;set;}
        public DbSet<Review> Reviews {get;set;}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Review>().HasKey(r => new { r.UserId, r.ProductId });

            var data =  new[] { new { value = 45, date = DateTime.Now }, new { value = 55, date = DateTime.Now } };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);   
            builder.Entity<Sensor>().HasData(
                new Sensor() { Id = -1, Name = "yaa", X = 100f, Y = 100f, Data = json, UpperValue = 50f, LowerValue = 10f, ProjectId = -1 },
                new Sensor() { Id = -2, Name = "yaa", X = 200f, Y = 200f, Data = json, UpperValue = 50f, LowerValue = 10f, ProjectId = -2 }
            );

            builder.Entity<UserProject>().HasKey(up => new { up.ProjectId, up.JineoUserId });
            builder.Entity<UserProject>().HasOne(up => up.Project).WithMany(p => p.UsersProjects).HasForeignKey(up => up.ProjectId);

            builder.Entity<IssueSensor>().HasKey(ise => new { ise.IssueId, ise.SensorId });
            builder.Entity<IssueSensor>().HasOne(ise => ise.Issue).WithMany(i => i.IssueSensors).HasForeignKey(ise => ise.IssueId);


            builder.Entity<Product>().HasData(
                new Product() { Id = -1, Name = "XAE 12", Description = "Innovational product that helps you to solve almost any problems.", ProductTypeId = 1, Image = "/img/sensor1.jpg" },
                new Product() { Id = -2, Name = "E 11", Description = "Pressure sensor, that allows you to manage your system 1-st class way.", ProductTypeId = 2, Image = "/img/sensor2.jpg" },
                new Product() { Id = -3, Name = "A 14", Description = "Temnerature sensor that requires no eyo on it to keep running.", ProductTypeId = 3, Image = "/img/sensor3.jpg" }
            );

            builder.Entity<Review>().HasData(
                new Review() { UserId = "3", ProductId = -1, Text = "Great", Mark = 5 }
            );

            builder.Entity<ProductLink>().HasData(
                new ProductLink() {Id = -1, Link = "https://bt.rozetka.com.ua/190067277/p190067277/?gclid=CjwKCAjwt-L2BRA_EiwAacX32StScd8-lgqKakD2LzongKsImKwwzEkuFgp_zc5FTCs4D8tOAeEGtxoCzR0QAvD_BwE", Price = 50, ProductId = -1, Store = "Rozetka" }
            );

            builder.Entity<Issue>().HasData(
                new Issue() { Id = -1, ProjectId = -1, UserId = "1", Title = "Alert!", Content = "That's bad", Status = "OPENED" }
            );
            builder.Entity<Comment>().HasData(
                new Comment(){ Id = -1, UserId = "1", Date = DateTime.Now, IssueId = -1, Text = "I am opened for your suggestions."}
            );
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
                new Project() { Id = -2,  Name = "Second project"}
                
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
