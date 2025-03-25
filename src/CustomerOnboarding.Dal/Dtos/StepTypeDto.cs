using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class StepTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public string FullTypeName { get; set;} = string.Empty;

        public string RuleSet { get; set; } = string.Empty;

        public string ComponentTypeName { get; set; } = string.Empty;
    }
}
