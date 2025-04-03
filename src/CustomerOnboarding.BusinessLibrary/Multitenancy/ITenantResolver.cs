using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Multitenancy
{
    public interface ITenantResolver<TTenant>
    {
        public TTenant? Tenant { get; }
    }
}
