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
