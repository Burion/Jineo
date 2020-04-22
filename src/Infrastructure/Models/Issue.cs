using System;

namespace Jineo.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public int ProjectId {get;set;}
        public Project Project {get;set;}
        public string UserID { get; set; }
        public JoneoUser User {get;set;}
        public string Content { get; set; }
        public int StatusID { get; set; }
    }
}