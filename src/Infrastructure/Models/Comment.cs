using System;

namespace Jineo.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public JineoUser User {get;set;}
        public int IssueId { get; set; }
        public Issue Issue {get;set;}
        public DateTime Date {get;set;}
        public string Text {get;set;}
    }
}