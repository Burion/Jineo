using System;

namespace Jineo.DTOs
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int SubscriptionId { get; set; }
        public bool IsBanned {get;set;}
        public string Role { get; set; }
        
    }
}