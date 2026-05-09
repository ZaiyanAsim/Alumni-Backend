using Alumni_Portal.Auth.Services.ResourceOwnership;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RequireProjectOwnerAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _routeParameter;

    public RequireProjectOwnerAttribute(string routeParameter = "id")
    {
        _routeParameter = routeParameter;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var alumniTokenValid = context.HttpContext.Items["AlumniTokenValid"] as bool? ?? false;

        if (!alumniTokenValid)
        {
            context.HttpContext.Items["AlumniCheckPassed"] = false;
            return;
        }

        // ✅ Read from stored AlumniPrincipal — no redundant AuthenticateAsync call
        var alumniPrincipal = context.HttpContext.Items["AlumniPrincipal"] as ClaimsPrincipal;

        if (alumniPrincipal == null)
        {
            context.HttpContext.Items["AlumniCheckPassed"] = false;
            return;
        }

        var userIdClaim = alumniPrincipal
            .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) ||
            !int.TryParse(userIdClaim, out int individualId))
        {
            context.HttpContext.Items["AlumniCheckPassed"] = false;
            return;
        }

        var projectId = context.HttpContext
            .Request.RouteValues[_routeParameter]?.ToString();

        if (string.IsNullOrEmpty(projectId))
        {
            context.HttpContext.Items["AlumniCheckPassed"] = false;
            return;
        }

        var ownershipService = context.HttpContext.RequestServices
            .GetRequiredService<ProjectOwnershipService>();

        var isOwner = await ownershipService.IsOwnerAsync(projectId, individualId);

        context.HttpContext.Items["AlumniCheckPassed"] = isOwner;

        if (isOwner)
            context.HttpContext.Items["ProjectId"] = projectId;
    }
}