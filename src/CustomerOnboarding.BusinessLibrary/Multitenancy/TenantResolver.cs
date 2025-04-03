using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Multitenancy
{
    public class TenantResolver(ApplicationContext appCtx,IDataPortal<TenantInfo>portal):ITenantResolver<TenantInfo>
    {
        private readonly ApplicationContext _appCtx=appCtx;
        private readonly IDataPortal<TenantInfo> _portal=portal;
        private TenantInfo tenant = default!;
        public TenantInfo? Tenant
        {
            get
            {
                if(tenant == null)
                {
                    ClaimsPrincipal? user = null;
                    user = _appCtx.Principal;
                    if(user is not null && user.HasClaim(c => c.Type == "TenantId"))
                    {
                        var claim=user.FindFirst(c => c.Type == "TenantId");
                        if(claim is not null)
                        {
                            tenant=_portal.Fetch(claim.Value);
                        }
                    }
                }
                return tenant;
            }
        }
    }
}
