using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class OrganisationProfileDto
    {
        public string TenantId { get; set; } = string.Empty;
        public string Name { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } =string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set;} = string.Empty;
        public byte[] LastChanged { get; set; } = default!;

    }
}
