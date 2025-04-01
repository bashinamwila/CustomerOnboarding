using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class EmailTemplateEntity
    {
        public int Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string AssemblyQualifiedName { get; set; } = string.Empty;
        public byte[] LastChanged { get; set; } = default!;
    }
}
