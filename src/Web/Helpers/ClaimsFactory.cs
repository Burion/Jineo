using System.Security.Claims;
using System.Threading.Tasks;
using Jineo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Jineo.Extentions
{
    public class JineoUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<JineoUser> 
    {     
        public JineoUserClaimsPrincipalFactory(
            UserManager<JineoUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)     
        {
            
        }
        
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(JineoUser user)
        {
            var email = user.Email;
            //var _user = await userManager.Users.Include(u => u.Company).SingleAsync(u => u.Email == email);
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("CompanyName", user.Company != null ? user.Company.Name : ""));         
            return identity;     
        } 
    }
}