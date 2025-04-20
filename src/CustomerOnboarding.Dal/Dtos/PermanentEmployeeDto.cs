using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class PermanentEmployeeDto
    {
        public int Id { get; set; }
        public string TenantId { get; set; } = String.Empty;
        public string EmployeeId { get; set; } = String.Empty;
        public byte[]? LastChanged { get; set; } = null;
    }
}
