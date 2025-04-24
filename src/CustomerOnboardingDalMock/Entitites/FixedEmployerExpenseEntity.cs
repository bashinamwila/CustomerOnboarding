using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class FixedEmployerExpenseEntity
    {
        public int Id { get; set; }
        public string EmployerExpenseId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public byte[] LastChanged { get; set; } = default!;
    }
}
