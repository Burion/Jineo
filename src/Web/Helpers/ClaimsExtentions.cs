using System.Security.Claims;
using System.Security.Principal;

namespace Jineo.Extentions
{
    public static class IdentityExtensions
    {
        public static bool IsEmployed(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CompanyName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? true : false;
        }
    }
}