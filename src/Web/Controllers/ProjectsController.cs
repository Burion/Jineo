using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jineo.Models;
using Jineo.ViewModels;
using AutoMapper;
using Jineo.Data;
using Jineo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Jineo.Logic;
using Jineo.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Jineo.MobileModels;
using System.Net.Mail;

namespace Jineo.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        readonly IMapper mapper;
        readonly ApplicationDbContext ctx;
        readonly UserManager<JineoUser> um;
        IWebHostEnvironment _env;
        public ProjectsController(IMapper _mapper, ApplicationDbContext _ctx, UserManager<JineoUser> um, IWebHostEnvironment appEnvironment)
        {
            mapper = _mapper;
            ctx = _ctx;
            ctx.Database.EnsureCreated();
            this.um = um;
            _env = appEnvironment;
        }
    
        
        [Route("projectusers")]
        public async Task<JsonResult> ProjectUsers(string id)
        {
            var project = ctx.Projects.Include(p => p.UsersProjects).ThenInclude(up => up.User).Single(p => p.Id == int.Parse(id));
            var users = project.UsersProjects.Select(up => up.User);
            var userDTOs = mapper.Map<UserDTO[]>(users);
            return new JsonResult(new { Users = userDTOs });
        }

        [Route("projects/{id}")]
        public async Task<IActionResult> Project(string id)
        {
            ProjectPageViewModel model = new ProjectPageViewModel();
            var project = ctx.Projects.Single(p => p.Id == int.Parse(id));
            var projectDTO = mapper.Map<ProjectDTO>(project);
            model.Products = mapper.Map<ProductDTO[]>(ctx.Products.ToArray());
            model.Project = projectDTO;
            return View(model);
        }

        public async Task<IActionResult> DeleteProject(string id)
        {
            ctx.Projects.Remove(ctx.Projects.Single(p => p.Id == int.Parse(id)));
            ctx.SaveChanges();
            return RedirectToAction("ProjectsPage");
        }
        [HttpGet]
        public async Task<IActionResult> NewProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewProject(string name, string desc, string type, IFormFile file)
        {
            var user = await um.FindByEmailAsync(User.Identity.Name);
            var project = new Project() { Name = name, Description = desc, Type = type, CreationDate = DateTime.Now.Date, OwnerEmail = User.Identity.Name };
            string path = "/img/" + file.FileName;
            using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            project.Image = path;
            ctx.Projects.Add(project);
            ctx.SaveChanges();
            ctx.UsersProjects.Add(new UserProject() { JineoUserId = user.Id, ProjectId = project.Id });
            ctx.SaveChanges();
            return RedirectToAction("ProjectsPage");
        }


        [Route("projects")]
        public async Task<JsonResult> Projects()
        {
            var user = await um.FindByEmailAsync(User.Identity.Name);
            var _projects = ctx.UsersProjects.Where(pu => pu.User.Id == user.Id).Select(pu => pu.Project);
            var projects = mapper.Map<ProjectDTO[]>(_projects);
            return new JsonResult(new { projects, max = 3 + user.SubscriptionId*2 });
        }

        public async Task<IActionResult> ProjectsPage() 
        {
            return View();
        }

        [Route("getproject")]
        public async Task<IActionResult> GetProject(string id)
        {
            var project = ctx.Projects.Single(p => p.Id == int.Parse(id));
            return new JsonResult(new { Project = project });
        }

        [Route("writeproject")]
        public async Task<IActionResult> WriteProject(string id, string json)
        {
            var project = ctx.Projects.Single(p => p.Id == int.Parse(id));
            project.Json = json;
            ctx.SaveChanges();
            return new JsonResult(new { Json = json });
        }

        [Route("projectinfo")]
        public async Task<JsonResult> Projects(string id)
        {
            var _project = ctx.Projects.Single(p => p.Id == int.Parse(id));
            var project = mapper.Map<ProjectDTO>(_project);
            return new JsonResult(new { project });
        }

        [Route("issues")]
        public async Task<JsonResult> Issues (string id)
        {
            var issues = ctx.Issues.Include(i => i.User).Include(i => i.Comments).ThenInclude(c => c.User).Include(i => i.IssueSensors).ThenInclude(ise => ise.Sensor).Where(i => i.ProjectId == int.Parse(id));
            var issuesDTO = mapper.Map<IssueDTO[]>(issues);

            return new JsonResult(new { issues = issuesDTO });
        }

        [Route("addcomment")]

        public IActionResult AddComment(string id, string content)
        {
            var user = ctx.Users.Single(u => u.UserName == User.Identity.Name);
            ctx.Comments.Add(new Comment() { Date  = DateTime.Now, IssueId = int.Parse(id), Text = content, UserId = user.Id });
            ctx.SaveChanges();
            return new JsonResult(new {status = "it's ok"});
        }

        [Route("addissue")]

        public IActionResult AddIssue(string projectid, string content, string title, string[] sensorsIds)
        {
            var issue = new Issue() { ProjectId = int.Parse(projectid), Content = content, Title = title, Status = "OPENED", OwnerEmail = User.Identity.Name };
            ctx.Issues.Add(issue);
            ctx.SaveChanges();

            List<IssueSensor> issueSensors = new List<IssueSensor>();
            foreach(var id in sensorsIds) 
            {
                issueSensors.Add(new IssueSensor() { IssueId = issue.Id, SensorId = int.Parse(id)});
            }
            issue.IssueSensors = issueSensors;

            ctx.SaveChanges();

            return new JsonResult(new {status = "it's ok"});
        }

        [AllowAnonymous]
        [Route("getsensors")]
        public JsonResult GetSensors(string projectId)
        {
            List<Sensor> s = new List<Sensor>();
            if(ctx.Sensors.Where(s => s.ProjectId == int.Parse(projectId)).Count() > 0)
            {
                s = ctx.Sensors.Where(s => s.ProjectId == int.Parse(projectId)).Include(s => s.Product).ToList();
            }
            var sensors = new SensorDTO[] { };
            if(s.Count() > 0)
                sensors = mapper.Map<SensorDTO[]>(s);
            return new JsonResult(new { sensors });
        }

        public IActionResult SendRequest()
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential("vladislavburyak@gmail.com", "vlad06021222a");
            // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("vladislavburyak@gmail.com");
            mail.To.Add(new MailAddress("vladyslav.buryak@nure.ua"));

            smtpClient.Send(mail);
            return Content("Email has been sent.");
        }
        [AllowAnonymous]
        [Route("getprojects/{email}")]
        public IActionResult GetProjectsByEmail(string email)
        {
            var user = ctx.Users.Single(u => u.Email == email);
            if(user == null)
                return new JsonResult(new { projects = new ProjectModel[] { } });
            var projects = ctx.UsersProjects.Where(up => up.User.Email == email).Select(up => up.Project);
            var projectsmodels = mapper.Map<ProjectModel[]>(projects);
            return new JsonResult(new { projects = projectsmodels });   

        }
        [AllowAnonymous]
        [Route("getsensors/{projectId}")]
        public IActionResult GetSensorsJson(string projectId)
        {
            var sensors = ctx.Sensors.Where(s => s.ProjectId == int.Parse(projectId)).ToList();
            var sensorsmodels = mapper.Map<SensorModel[]>(sensors);
            for(int x = 0; x < sensors.Count(); x++)
            {
                var meterings = Newtonsoft.Json.JsonConvert.DeserializeObject<MeteringModel[]>(sensors[x].Data);
                var last = meterings.Last();
                if(last.value > sensors[x].UpperValue || last.value < sensors[x].LowerValue)
                    sensorsmodels[x].Status = "DANGER";
                else
                    sensorsmodels[x].Status = "OK";
                string h = last.date.Hour > 9 ? last.date.Hour.ToString() : '0' + last.date.Hour.ToString();
                string m = last.date.Minute > 9 ? last.date.Minute.ToString() : '0' + last.date.Minute.ToString();;
                string mo = last.date.Month > 9 ? last.date.Month.ToString() : '0' + last.date.Month.ToString();
                string d = last.date.Day > 9 ? last.date.Day.ToString() : '0' + last.date.Day.ToString();

                string date = $"{h}:{m} {d}/{mo}";
                sensorsmodels[x].Date = date;
            }
            return new JsonResult(new { sensors = sensorsmodels });
        }
        [Route("addsensor")]
        public JsonResult AddSensor(string projectId, string x, string y, string name, string upperValue, string lowerValue, string productId) 
        {
            Random r = new Random();
            List<MeteringModel> meterings = new List<MeteringModel>();
            for(int a = 0; a < 30; a++)
            {
                meterings.Add(new MeteringModel { value = r.Next(50, 100), date = DateTime.Now });
            }
            var json = meterings.ToArray();
            var _json = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            // TODO FIX PRODUCTID
            ctx.Sensors.Add(new Sensor() { ProductId = int.Parse(productId), ProjectId = int.Parse(projectId), Name = name, X = float.Parse(x), Y = float.Parse(y), UpperValue = int.Parse(upperValue), LowerValue = int.Parse(lowerValue), Data = _json });
            ctx.SaveChanges();
            return new JsonResult( new { Message = "Sensor was added" });
        }

        [Route("getcomments")]
        public JsonResult GetComments(string id)
        {
            var comments_ = ctx.Issues.Include(i => i.Comments).ThenInclude(c => c.User).Single(i => i.Id == int.Parse(id)).Comments;
            var comments = mapper.Map<CommentDTO[]>(comments_);
            return new JsonResult( new { comments });
        }

        [Route("changestatus")]
        public JsonResult ChangeStatus(string issueId)
        {
            var issue = ctx.Issues.Single(i => i.Id == int.Parse(issueId));
            var status = issue.Status == "OPENED" ? "CLOSED" : "OPENED";
            issue.Status = status;
            ctx.SaveChanges();
            return new JsonResult(new { status, Id = issue.Id });
        }

        [Route("deleteuserfromproject")]
        public async Task<IActionResult> DeleteUserFromProject(string email, string projectId)
        {
            var user = await um.FindByEmailAsync(email);
            var userProject = ctx.UsersProjects.Single(up => up.JineoUserId == user.Id && up.ProjectId == int.Parse(projectId));
            ctx.UsersProjects.Remove(userProject);
            await ctx.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "User was deleted from project "}); 
        }

        [Route("analyze")]
        public async Task<IActionResult> Analyze(string _json)
        {
            
            var blocks = Newtonsoft.Json.JsonConvert.DeserializeObject<DataBlock[]>(_json).ToList();
            var cells = DataAnalizer.AnalizeData(blocks, 4);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(cells);
            Console.WriteLine(json);
            ViewBag.Json = json;
            return View("Analyze", new AnalyzeDataModel() { Json = json.ToString() });
        }
        [HttpGet]
        public async Task<IActionResult> UploadAnalyzeFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAnalyzeFile(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine()); 
            }
            Console.WriteLine(result);
            return await Analyze(result.ToString());
        }

        [Route("addusertoproject")]
        public async Task<ActionResult> AddUserToProject(string email, string projectId)
        {
            var user = await um.FindByEmailAsync(email);
            if(user == null)
                return new JsonResult(new { success = false, message = "User is not found. Check email again." }); 
            if(user.SubscriptionId*2 + 3 <= ctx.UsersProjects.Where(up => up.JineoUserId == user.Id).Count())
                return new JsonResult(new { success = false, message = "The limit of maximum projects is reached by this user." }); 
            var record = new UserProject() { JineoUserId = user.Id, ProjectId = int.Parse(projectId)};
            var list = ctx.UsersProjects.Where(up => up.ProjectId == record.ProjectId && up.JineoUserId == record.JineoUserId).Count();

            if(list == 0)
            {
                ctx.UsersProjects.Add(record);
                await ctx.SaveChangesAsync();
            }
            else
            {
                return new JsonResult(new { success = false, message = "Project already contains this user" }); 
            }
            
            return new JsonResult(new { success = true, record = new { email = email }, message = "User is added" }); 
        }
    }

}