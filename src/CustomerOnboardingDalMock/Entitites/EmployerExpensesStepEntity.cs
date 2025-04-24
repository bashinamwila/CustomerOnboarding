using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class EmployerExpensesStepEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public int Id { get; set; }
        public int StepIndex { get; set; }
        public bool IsCompleted { get; set; }
        public bool Skip { get; set; }
        public int CurrentStepIndex { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}

