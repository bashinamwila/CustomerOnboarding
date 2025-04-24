using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class SelectEmployeeWageStepDto
    {
        public string TenantId { get; set; } = string.Empty;
        public int StepId { get; set; }
        public int StepIndex { get; set; }
        public bool IsCompleted { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public string WageId { get; set; } = string.Empty;
        public byte[] LastChanged { get; set; } = default!;
        public int Counter { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
    }
}
