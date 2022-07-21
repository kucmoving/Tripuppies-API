using System.Security.Claims;

namespace API.Helpers
{
    //static 
    public static class ClaimsPrinicipleExtensions
    {
        //return name
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        //return name
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

    }
}
