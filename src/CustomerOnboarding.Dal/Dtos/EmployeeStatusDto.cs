using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class EmployeeStatusDto
    {
        public string TenantId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public int Id { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
