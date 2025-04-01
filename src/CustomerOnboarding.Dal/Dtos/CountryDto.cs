using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class CountryDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
        public byte[] LastChanged { get; set; } = default!;
    }
}
