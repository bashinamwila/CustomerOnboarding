using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class InActiveEmployeeStatusDto
    {
        public string TenantId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public int ReasonForInActivity { get; set; }
        public DateTime DateOfInActivity { get; set; }
        public int Id { get; set; }
        public bool IsOnPayroll { get; set; }
        public byte[] LastChanged { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
    }
}
