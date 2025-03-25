using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class StepEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public string RuleSet { get; set; } = string.Empty;
        public string FullTypeName { get; set;} = string.Empty;
        public byte[] LastChanged { get; set; } = default!;
    }
}
