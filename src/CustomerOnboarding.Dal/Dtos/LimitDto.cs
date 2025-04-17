using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class LimitDto
    {
        public int Id { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public string ItemId { get; set; } = String.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public int PayrollId { get; set; }
        public decimal? Minimum { get; set; }
        public decimal? Maximum { get; set; }
        public decimal? Value { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
