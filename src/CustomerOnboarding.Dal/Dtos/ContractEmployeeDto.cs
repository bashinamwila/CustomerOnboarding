using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class ContractEmployeeDto
    {
        public int Id { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = String.Empty;
        public int? ContractDuration { get; set; }
        public int? Interval { get; set; }
        public DateTime ContractStartDate { get; set; }
        public byte[]? LastChanged { get; set; } = null;
    }
}
