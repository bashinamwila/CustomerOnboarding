using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class DeductionEntity
    {
        public string Id { get; set; } = string.Empty;

        public string TenantId { get; set; } = string.Empty;
        public string Name { get; set; } = String.Empty;
        public bool IsSystemDefined { get; set; }
        public int Type { get; set; }
        public int Limit { get; set; }
        public byte[] LastChanged { get; set; } = default!;
    }
}
