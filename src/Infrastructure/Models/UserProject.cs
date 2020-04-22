using System;

namespace Jineo.Models
{
    public class UserProject    
    {
        public string JineoUserId {get;set;}
        public JineoUser User {get;set;}
        
        public int ProjectId {get;set;}
        public Project Project {get;set;}
    }
}