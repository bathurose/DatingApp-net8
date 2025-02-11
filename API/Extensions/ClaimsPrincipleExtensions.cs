using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userName = user.FindFirstValue(ClaimTypes.NameIdentifier) 
                ?? throw new Exception("Cannot find the username");
            return userName;
        }
    }
}
