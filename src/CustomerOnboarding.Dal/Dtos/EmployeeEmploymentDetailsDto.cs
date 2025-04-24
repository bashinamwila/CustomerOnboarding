using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class EmployeeEmploymentDetailsDto
    {
        public string TenantId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? StatusId { get; set; }
        public int? PayType { get; set; }
        public int? EmploymentType { get; set; }
        public DateTime? HireDate { get; set; }
        public int? Group { get; set; }
        public int? DeptId { get; set; }
        public int? JobTitle { get; set; }
        public int? ReportsTo { get; set; }
        public string? ReportsToEmployeeId { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
