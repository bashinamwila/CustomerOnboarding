using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class StatutoryRegistrationsDto
    {
        public string TPIN { get; set; } = string.Empty;
        public string NAPSAAccountNumber { get; set; } = string.Empty;
        public string NHIMAAccountNumber { get; set; } = string.Empty;

        public string TenantId { get; set; } = string.Empty;

        public byte[] LastChanged { get; set; } = default!;
    }
}
