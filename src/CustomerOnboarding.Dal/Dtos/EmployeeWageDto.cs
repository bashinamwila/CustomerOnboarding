using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class EmployeeWageDto
    {
        public string TenantId { get; set; } = string.Empty;
        public string WageId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public decimal YTD { get; set; }
        public string Formular { get; set; } = string.Empty;
        public int Type { get; set; }
        public bool IsSystemDefined { get; set; }
        public byte[] LastChanged { get; set; } = default!;

    }
}
