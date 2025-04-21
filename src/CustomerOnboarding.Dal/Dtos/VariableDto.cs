using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    [Serializable]
    public class VariableDto
    {
        public int Id { get; set; }
        public string ItemId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public decimal? Value { get; set; }
        public byte[] LastChanged { get; set; } = default!;

    }
}
