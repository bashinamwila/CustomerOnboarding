using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class TypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public string FriendlyName { get; set; } = string.Empty;
        public int Type { get; set; }
        public string ComponentTypeName { get; set; } = string.Empty;
        public string DerivativeTypeName { get; set; } = string.Empty;
        public byte[]? LastChanged { get; set; } = default!;

    }
}
