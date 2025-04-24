using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class TenantOnboardingEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public int CurrentStepIndex { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
