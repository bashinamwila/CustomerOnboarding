using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class CreateAccountStepDto
    {
        public int StepIndex { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public string WorkEmail { get; set; } = string.Empty;
        public string OrganisationPhone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
         public string OrganisationName { get; set; }=string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public byte[] LastChanged { get; set; } = default!;
        public string Name { get; set;} = string.Empty;
        public int Type { get; set; }
        public bool IsCompleted { get; set; }
    }
}
