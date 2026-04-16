using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.TenantService
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
            
            var clientIdClaim = context.User?.FindFirst("Client_ID")?.Value;

            if (!string.IsNullOrEmpty(clientIdClaim) && int.TryParse(clientIdClaim, out int clientId))
            {
                tenantService.SetTenant(clientId);
            }

            await _next(context);
        }
    }

}
