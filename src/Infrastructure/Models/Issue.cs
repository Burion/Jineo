using System;
using System.Collections.Generic;

namespace Jineo.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public int ProjectId {get;set;}
        public Project Project {get;set;}
        public string UserId { get; set; }
        public JineoUser User {get;set;}
        public string OwnerEmail {get;set;}
        public string Content { get; set; }
        public string Title {get;set;}
        public string Status { get; set; }
        public IEnumerable<Comment> Comments {get;set;}
        public IEnumerable<IssueSensor> IssueSensors {get;set;}
    }
}