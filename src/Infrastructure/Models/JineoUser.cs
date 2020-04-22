using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Jineo.Models
{
    public class JineoUser: IdentityUser
    {
        public string Login { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    
    }
}