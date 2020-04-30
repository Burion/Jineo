using System;

namespace Jineo.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserDTO User {get;set;}
        public DateTime Date {get;set;}
        public string Text {get;set;}
    }
}