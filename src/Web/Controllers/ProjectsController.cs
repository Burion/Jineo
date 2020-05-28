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

namespace Jineo.Controllers
{
    public class ProjectsController : Controller
    {
        readonly IMapper mapper;
        readonly ApplicationDbContext ctx;
        readonly UserManager<JineoUser> um;
        public ProjectsController(IMapper _mapper, ApplicationDbContext _ctx, UserManager<JineoUser> um)
        {
            mapper = _mapper;
            ctx = _ctx;
            ctx.Database.EnsureCreated();
            this.um = um;
        }
    
        // public IActionResult Projects()
        // {        
        //     var model = new ProjectsListPageViewModel() { Message = "hello"};
        //     model.Projects = mapper.Map<List<ProjectDTO>>(ctx.Projects);
        //     model.Message = ctx.Projects.Count().ToString();
        //     var user = um.FindByEmailAsync(User.Identity.Name).Result;
        //     Console.WriteLine($"USER ROLES: {um.GetRolesAsync(user).Result.First()}");
        //     Console.WriteLine(User.IsInRole("SuperU"));
        //     return View(model);
        // }
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
            model.Project = projectDTO;
            return View(model);
        }


        [Route("projects")]
        public async Task<JsonResult> Projects()
        {
            var user = await um.FindByEmailAsync(User.Identity.Name);
            var _projects = ctx.UsersProjects.Where(pu => pu.User.Id == user.Id).Select(pu => pu.Project);
            var projects = mapper.Map<ProjectDTO[]>(_projects);
            return new JsonResult(new { projects });
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
            var issues = ctx.Issues.Include(i => i.User).Include(i => i.Comments).Include(i => i.IssueSensors).ThenInclude(ise => ise.Sensor).Where(i => i.ProjectId == int.Parse(id));
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
            var issue = new Issue() { ProjectId = int.Parse(projectid), Content = content, Title = title };
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

        [Route("getsensors")]
        public JsonResult GetSensors(string projectId)
        {
            var sensors = mapper.Map<SensorDTO[]>(ctx.Sensors);
            return new JsonResult(new { sensors });
        }

        [Route("addsensor")]
        public JsonResult AddSensor(string projectId, string x, string y, string name, string upperValue, string lowerValue) 
        {
            var json = new[] { new { value = 40, date = DateTime.Now }, new { value = 45, date = DateTime.Now } };
            var _json = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            ctx.Sensors.Add(new Sensor() { ProjectId = int.Parse(projectId), Name = name, X = float.Parse(x), Y = float.Parse(y), UpperValue = int.Parse(upperValue), LowerValue = int.Parse(lowerValue), Data = _json });
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
            // Random r = new Random();
            // List<DataBlock> blocks = new List<DataBlock>();
            // for(int x = 0; x < 10; x++) 
            // {
            //     for(int y = 0; y < 10; y++)
            //     {
            //         var dencity = (float)r.Next(13, 22) / 10;
            //         var pressure = (float)r.Next(9, 30) / 10;
            //         var waterproof = (float)r.Next(1, 100) / 100;
            //         var dict = new Dictionary<int, float>();
            //         dict.Add((int)MeteringType.Density, dencity);
            //         dict.Add((int)MeteringType.Pressure, pressure);
            //         dict.Add((int)MeteringType.Waterproof, waterproof);
            //         blocks.Add(new DataBlock() { Data = dict, X = x, Y = y });
            //     }
            // }
            // var cells = DataAnalizer.AnalizeData(blocks, 4);
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(cells);
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