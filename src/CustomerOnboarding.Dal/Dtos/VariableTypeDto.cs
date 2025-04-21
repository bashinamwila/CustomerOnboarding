using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class VariableTypeDto
    {
        public int Id { get; set; }
        public string Pattern { get; set; } = string.Empty;
        public byte[]? LastChanged { get; set; } = default!;
        public string TypeName { get; set; } = string.Empty;
        public string ComponentTypeName { get; set; } = string.Empty;
    }
}
