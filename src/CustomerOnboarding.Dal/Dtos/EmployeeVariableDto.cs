using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class EmployeeVariableDto
    {
        public int Id { get; set; }

        public string TenantId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public decimal? Value { get; set; } = default!;
        public string Token { get; set; } = string.Empty;
        public byte[] LastChanged { get; set; } = default!;
    }
}
