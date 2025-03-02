using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userName = user.FindFirstValue(ClaimTypes.Name) 
                ?? throw new Exception("Cannot find the username");
            return userName;
        }

        public static int GetUseId(this ClaimsPrincipal user)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("Cannot find the username"));
            return userId;
        }
    }
}
