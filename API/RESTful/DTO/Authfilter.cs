using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthFilter : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var path = context.HttpContext.Request.Path.Value;
        if (path.Contains("/swagger"))
        {
            return;
        }
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedObjectResult("Je moet ingelogd zijn om van de functies gebruik te maken.");
        }
    }
}