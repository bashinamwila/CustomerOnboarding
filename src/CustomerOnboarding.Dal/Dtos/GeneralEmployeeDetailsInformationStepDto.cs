using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class GeneralEmployeeDetailsInformationStepDto
    {
        public string TenantId { get; set; } = string.Empty;
        public int StepId { get; set; }
        public int StepIndex { get; set; }
        public bool IsCompleted { get; set; }
        public byte[] LastChanged { get; set; } = default!;
        public string Name { get; set; } = string.Empty;

        public int Type { get; set; }
    }
}
