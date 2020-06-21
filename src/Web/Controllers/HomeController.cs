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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace Jineo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly ApplicationDbContext ctx;
        readonly IMapper mapper;
        
        
        IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext ctx, IMapper mapper, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            this.ctx = ctx;
            this.mapper = mapper;
            _env = appEnvironment;
        }

        

        public IActionResult ChangeLocale(string locale)
        {   

            var cultureInfo = new CultureInfo(locale);  
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            return RedirectToAction("Index");

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddLink(string store, string link, string price, string productId)
        {
            ProductLink _link = new ProductLink() { Link = link, Store = store, Price = float.Parse(price), ProductId = int.Parse(productId) };
            ctx.ProductLinks.Add(_link);
            ctx.SaveChanges();
            return RedirectToAction("ItemPage", new { id = int.Parse(productId) });
        }

        [Route("home/store/{id}")]
        public IActionResult ItemPage(string id)
        {
            var product = ctx.Products.Include(p => p.Links).Include(p => p.Reviews).ThenInclude(r => r.User).Single(p => p.Id == int.Parse(id));
            var productDTO = mapper.Map<ProductDTO>(product);
            if(ctx.ProductLinks.Where(pl => pl.ProductId == productDTO.Id).Count() > 0)
            {    
                productDTO.AvgPrice = (int)ctx.ProductLinks.Where(pl => pl.ProductId == productDTO.Id).Average(pl => pl.Price);
            }

            if(ctx.Reviews.Where(r => r.ProductId == productDTO.Id).Count() > 0)
            {    
                productDTO.AvgMark = (int)ctx.Reviews.Where(r => r.ProductId == productDTO.Id).Average(pl => pl.Mark);
            }
            productDTO.Running = ctx.Sensors.Where(s => s.ProductId == productDTO.Id).Count();
            return View(productDTO);
        }

        public async Task<IActionResult> DeleteProductLink(string linkId)
        {
            var link = ctx.ProductLinks.Single(pl => pl.Id == int.Parse(linkId));
            var productId = link.ProductId;
            ctx.ProductLinks.Remove(link);
            ctx.SaveChanges();
            return RedirectToAction("ItemPage", new { id = productId });
        }
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            var p = ctx.Products.Single(p => p.Id == int.Parse(productId));
            ctx.Products.Remove(p);
            ctx.SaveChanges();
            return RedirectToAction("Store");
        }
        public async Task<IActionResult> DeleteReview(string productId, string userId)
        {
            var r = ctx.Reviews.Single(r => r.ProductId == int.Parse(productId) && r.UserId == userId);
            ctx.Reviews.Remove(r);
            ctx.SaveChanges();
            return RedirectToAction("ItemPage", new { id = int.Parse(productId)});
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
                    itemsDTO[x].AvgPrice = (int)ctx.ProductLinks.Where(pl => pl.ProductId == itemsDTO[x].Id).Average(pl => pl.Price);
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

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(string name, string desc, string type, IFormFile file)
        {
            var user = ctx.Users.Single(u => u.Email == User.Identity.Name);
            var product = new Product() { Name = name, Description = desc, ProductTypeId = int.Parse(type) };
            string path = "/img/" + file.FileName;
            using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            product.Image = path;
            ctx.Products.Add(product);
            ctx.SaveChanges();
            return RedirectToAction("Store");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
