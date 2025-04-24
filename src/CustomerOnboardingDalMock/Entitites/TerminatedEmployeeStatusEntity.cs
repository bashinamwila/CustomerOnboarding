using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class TerminatedEmployeeStatusEntity
    {

        public string TenantId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public int Id { get; set; }

        public int ReasonForTermination { get; set; }

        public DateTime DateOfTermination { get; set; }
       
        public byte[] LastChanged { get; set; } = default!;
    }
}
