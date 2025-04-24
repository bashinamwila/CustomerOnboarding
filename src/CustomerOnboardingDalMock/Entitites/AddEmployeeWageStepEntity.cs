using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class AddEmployeeWageStepEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public int Id { get; set; }
        public string WageId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public int StepIndex { get; set; }
        public bool IsCompleted { get; set; }
        public byte[] LastChanged { get; set; } = default!;
       
    }
}
