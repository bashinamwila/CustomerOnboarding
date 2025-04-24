using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class OrganisationProfileStepDto
    {
        public string TenantId { get; set; } = string.Empty;
        public int StepId { get; set; }
        public string RuleSet { get; set; } = string.Empty;
        public int StepIndex { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int Type { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
