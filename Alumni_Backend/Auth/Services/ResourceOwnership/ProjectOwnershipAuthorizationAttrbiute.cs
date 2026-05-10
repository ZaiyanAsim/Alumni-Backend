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
        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILogger<RequireProjectOwnerAttribute>>();

        var alumniTokenValid = context.HttpContext.Items["AlumniTokenValid"] as bool? ?? false;

        if (!alumniTokenValid)
        {
            context.HttpContext.Items["AlumniCheckPassed"] = false;
            return;
        }

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
            logger.LogWarning("Individual ID claim missing or invalid");
            context.HttpContext.Items["AlumniCheckPassed"] = false;
            return;
        }

        // ← Parse as int since project ID is integer
        var routeValue = context.HttpContext
            .Request.RouteValues[_routeParameter]?.ToString();

        if (string.IsNullOrEmpty(routeValue) ||
            !int.TryParse(routeValue, out int projectId))
        {
            logger.LogWarning("Route parameter '{Param}' is missing or not a valid integer",
                _routeParameter);
            context.HttpContext.Items["AlumniCheckPassed"] = false;
            return;
        }

        logger.LogInformation(
            "Checking ownership — IndividualId: {IndividualId}, ProjectId: {ProjectId}",
            individualId, projectId);

        var ownershipService = context.HttpContext.RequestServices
            .GetRequiredService<ProjectOwnershipService>();

        var isOwner = await ownershipService.IsOwnerAsync(projectId, individualId);

        logger.LogInformation("Ownership check result: {IsOwner}", isOwner);

        context.HttpContext.Items["AlumniCheckPassed"] = isOwner;

        if (isOwner)
            context.HttpContext.Items["ProjectId"] = projectId; // ← stored as int
    }
}