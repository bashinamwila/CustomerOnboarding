using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class RangeDeductionLimitEntity
    {
        public int Id { get; set; }
        public string DeductionId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
