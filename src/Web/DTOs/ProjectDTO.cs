using System;

namespace Jineo.DTOs 
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type {get;set;}
        public string Description {get;set;}
        public string Json {get;set;}
        public string Image {get;set;}
        public DateTime CreationDate { get; set; }
    }
}