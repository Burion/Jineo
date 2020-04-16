using System.Security.Claims;
using System.Threading.Tasks;
using Jineo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

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
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("CompanyName", user.Company != null ? user.Company.Name : null));         
            return identity;     
        } 
    }
}