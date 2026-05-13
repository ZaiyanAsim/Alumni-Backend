using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Alumni_Portal.Auth.Services.FeaturePermission;

namespace Alumni_Portal.Auth.Services.FeaturePermission
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _permission;

        public RequirePermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<RequirePermissionAttribute>>();

       
            ClaimsPrincipal? adminPrincipal = null;

        
            if (context.HttpContext.Items.ContainsKey("AdminTokenValid"))
            {
                var adminTokenValid = context.HttpContext.Items["AdminTokenValid"] as bool? ?? false;
                if (adminTokenValid)
                    adminPrincipal = context.HttpContext.Items["AdminPrincipal"] as ClaimsPrincipal;
            }
           
            else if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                adminPrincipal = context.HttpContext.User;
            }

       
            if (adminPrincipal == null)
            {
                logger.LogWarning("No admin principal — 401");
                context.Result = new UnauthorizedResult();
                return;
            }

            // ── Get UserId from token ────────────────────────────────
            var userId = adminPrincipal.FindFirst("UserId")?.Value
                      ?? adminPrincipal.FindFirst("userId")?.Value
                      ?? adminPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("UserId claim missing — 401");
                context.Result = new UnauthorizedResult();
                return;
            }

       
            var authHeader = context.HttpContext.Request
                .Headers["Authorization"].ToString();
            var jwtToken = authHeader.Substring("Bearer ".Length).Trim();

            var permissionService = context.HttpContext.RequestServices
                .GetRequiredService<AdminPermissionService>();

            var permissions = await permissionService.GetPermissionsAsync(userId, jwtToken);

            if (permissions == null)
            {
                logger.LogError("Could not fetch permissions for userId {UserId}", userId);
                context.Result = new ObjectResult("Could not verify permissions")
                { StatusCode = 503 };
                return;
            }

            logger.LogInformation("Permissions: [{Permissions}]",
                string.Join(", ", permissions.FeaturePermissionKeys));

            var passed = permissions.FeaturePermissionKeys.Contains(_permission);

            logger.LogInformation("Permission '{Permission}': {Result}",
                _permission, passed ? "PASSED" : "FAILED");

    
            if (!passed)
            {
                logger.LogWarning("Permission '{Permission}' denied — 403", _permission);
                context.Result = new ObjectResult(new
                {
                    error = $"Missing permission: {_permission}"
                })
                { StatusCode = 403 };
            }
        }
    }
}