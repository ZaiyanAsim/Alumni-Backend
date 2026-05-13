using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Alumni_Portal.Auth.Configuration
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireAdminOrAlumniAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public int Order => int.MinValue;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var logger = httpContext.RequestServices
                .GetRequiredService<ILogger<RequireAdminOrAlumniAttribute>>();

            // ── Decode raw token for debugging ──────────────────────
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var rawToken = authHeader.Substring("Bearer ".Length).Trim();
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

                if (handler.CanReadToken(rawToken))
                {
                    var decoded = handler.ReadJwtToken(rawToken);
                    logger.LogInformation("=== RAW TOKEN CLAIMS ===");
                    foreach (var claim in decoded.Claims)
                        logger.LogInformation("  {Type} = {Value}", claim.Type, claim.Value);
                    logger.LogInformation("=== END RAW TOKEN CLAIMS ===");
                }
                else
                {
                    logger.LogWarning("Token cannot be read — malformed?");
                }
            }

            // ── Try both schemes ─────────────────────────────────────
            var adminResult = await httpContext.AuthenticateAsync("AdminBearer");
            var alumniResult = await httpContext.AuthenticateAsync("AlumniBearer");

            var adminValid = adminResult.Succeeded;
            var alumniValid = alumniResult.Succeeded;

            logger.LogInformation(
                "AdminBearer: {AdminValid} | AlumniBearer: {AlumniValid}",
                adminValid, alumniValid);

            // Log why admin failed if it did
            if (!adminValid && adminResult.Failure != null)
                logger.LogWarning("AdminBearer failed: {Reason}", adminResult.Failure.Message);

            // ── Neither token valid → 401 ────────────────────────────
            if (!adminValid && !alumniValid)
            {
                logger.LogWarning("Both tokens invalid — returning 401");
                context.Result = new UnauthorizedResult();
                return;
            }

            // ── Store results in Items for downstream attributes ──────
            httpContext.Items["AdminTokenValid"] = adminValid;
            httpContext.Items["AlumniTokenValid"] = alumniValid;

            if (adminValid)
                httpContext.Items["AdminPrincipal"] = adminResult.Principal!;
            if (alumniValid)
                httpContext.Items["AlumniPrincipal"] = alumniResult.Principal!;

            // ── Set HttpContext.User to admin if valid, else alumni ───
            if (adminValid)
                httpContext.User = adminResult.Principal!;
            else
                httpContext.User = alumniResult.Principal!;

            logger.LogInformation(
                "Auth complete — AdminTokenValid: {Admin}, AlumniTokenValid: {Alumni}",
                adminValid, alumniValid);
        }
    }
}