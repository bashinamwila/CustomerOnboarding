using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class WageEntity
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        // public int WageType { get; set; }
        public bool IsSystemDefined { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public byte[] LastChanged { get; set; } = default!;
        public string Formular { get; set; } = string.Empty;
    }
}
