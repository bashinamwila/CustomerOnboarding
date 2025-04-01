using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class ConfirmEmailStepEntity
    {
        public int Id { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public int StepIndex { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
