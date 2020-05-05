using System;
using System.Collections.Generic;

namespace Jineo.DTOs
{
    public class IssueDTO
    {
        public int Id { get; set; }
        public int ProjectId {get;set;}
        public ProjectDTO Project {get;set;}
        public string UserID { get; set; }
        public UserDTO User {get;set;}
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public IEnumerable<CommentDTO> Comments {get;set;}
        public IEnumerable<IssueSensorDTO> IssueSensors {get;set;}
    }
}