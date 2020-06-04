using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jineo.Models;
using Microsoft.AspNetCore.Authorization;
using Jineo.ViewModels;
using Jineo.Data;
using AutoMapper;
using Jineo.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Jineo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly ApplicationDbContext ctx;
        readonly IMapper mapper;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext ctx, IMapper mapper)
        {
            _logger = logger;
            this.ctx = ctx;
            this.mapper = mapper;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        [Route("home/store/{id}")]
        public IActionResult ItemPage(string id)
        {
            var product = ctx.Products.Include(p => p.Links).Include(p => p.Reviews).ThenInclude(r => r.User).Single(p => p.Id == int.Parse(id));
            var productDTO = mapper.Map<ProductDTO>(product);
            if(ctx.ProductLinks.Where(pl => pl.ProductId == productDTO.Id).Count() > 0)
            {    
                productDTO.AvgPrice = ctx.ProductLinks.Where(pl => pl.ProductId == productDTO.Id).Average(pl => pl.Price);
            }

            if(ctx.Reviews.Where(r => r.ProductId == productDTO.Id).Count() > 0)
            {    
                productDTO.AvgMark = (int)ctx.Reviews.Where(r => r.ProductId == productDTO.Id).Average(pl => pl.Mark);
            }
            return View(productDTO);
        }

        public IActionResult AddReview(ReviewDTO review)
        {
            var user = ctx.Users.Single(u => u.Email == User.Identity.Name);
            review.UserId = user.Id;
            var r = mapper.Map<Review>(review);
            ctx.Reviews.Add(r);
            ctx.SaveChanges();
            return RedirectToAction("ItemPage", new { id = review.ProductId.ToString() });
        } 
        public IActionResult Store()
        {
            var items = ctx.Products.Include(p => p.Links).ToArray();
            var itemsDTO = mapper.Map<ProductDTO[]>(items);
            for(int x = 0; x < itemsDTO.Length; x++)
            {
                if(ctx.ProductLinks.Where(pl => pl.ProductId == itemsDTO[x].Id).Count() > 0)
                {    
                    itemsDTO[x].AvgPrice = ctx.ProductLinks.Where(pl => pl.ProductId == itemsDTO[x].Id).Average(pl => pl.Price);
                }
                if(ctx.Reviews.Where(pl => pl.ProductId == itemsDTO[x].Id).Count() > 0)
                {    
                    itemsDTO[x].AvgMark = (int)ctx.Reviews.Where(pl => pl.ProductId == itemsDTO[x].Id).Average(pl => pl.Mark);
                }
            }
            var model = new StoreModel() { Products = itemsDTO };
            return View(model);
        }
        public IActionResult Banned()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Newcomer()
        {
            
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
