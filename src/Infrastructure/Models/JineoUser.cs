using System;
using Microsoft.AspNetCore.Identity;

namespace Jineo.Models
{
    public class JineoUser: IdentityUser
    {
        public string Login { get; set; }
    }
}