using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.TenantService
{
    public interface ITenantService
    {
        int? TenantId { get; }
        void SetTenant(int tenantId);
    }

    public class TenantService : ITenantService
    {
        private int? _tenantId;

        public int? TenantId => _tenantId;

        public void SetTenant(int tenantId)
        {
            _tenantId = tenantId;
        }
    }

}
