using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jineo.Models;
using Jineo.ViewModels;

namespace Jineo.Controllers
{
    public class ProjectsController : Controller
    {
        public ProjectsController()
        {
        
        }
    
        public IActionResult Projects()
        {
            var model = new ProjectPageViewModel() { Message = "hello"};
            return View(model);
        }


    }
}