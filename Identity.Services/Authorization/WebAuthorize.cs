using Identity.Services.Authorization.Const;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Authentication;

namespace Identity.Services.Authorization
{
    [AttributeUsage(AttributeTargets.All)]
    public class WebAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly  int[] _actionAccess;

        public WebAuthorize(params int[] actionAccess)
        {
            _actionAccess = actionAccess;
        }



        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isLoggedIn = GetIsLoggedIn(context.HttpContext);
            if (!isLoggedIn)
                throw new AuthenticationException(AuthorizeMessageConsts.NotLoggedIn);

            var userAccess = GetUserClaims(context.HttpContext);
            if (_actionAccess.Any(a => userAccess.Contains(a)))
            {
                throw new AuthenticationException(AuthorizeMessageConsts.NotAccess);
            }
        }

        public static bool GetIsLoggedIn(HttpContext context)
        {
            return context.User != null
                        && context.User.Identity.IsAuthenticated;
        }
        public static string GetUserClaim(HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(p => p.Type == AuthorizePermissionConsts.User.UserAccess)?.Value;
        }

        private static IEnumerable<int> GetUserClaims(HttpContext context)
        {
            var userClaims = GetUserClaim(context);
            return userClaims.Split(",").ToList().ConvertAll(int.Parse);
        }
    }
}
