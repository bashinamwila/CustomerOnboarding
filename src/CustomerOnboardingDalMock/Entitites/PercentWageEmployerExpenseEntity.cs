using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class PercentWageEmployerExpenseEntity
    {
        public int Id { get; set; }
        public string EmployerExpenseId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;

        public decimal Percent { get; set; }

        public string WageId { get; set; } = string.Empty;

        public byte[] LastChanged { get; set; } = default!;
    }
}

