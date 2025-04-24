using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class InActiveEmployeeStatusEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public int Id { get; set; }

        public int ReasonForInActivity { get; set; }

        public DateTime DateOfInActivity { get; set; }
        public bool IsOnPayroll { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
