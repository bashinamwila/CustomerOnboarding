using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class StepDto
    {
        public int Id { get; set; }
        public string TenantId { get; set; } = string.Empty;
    }
}
