using System;
using System.Collections.Generic;

namespace Jineo.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public string Json { get; set; }
        public IEnumerable<UserProject> UsersProjects {get;set;}

    }
}