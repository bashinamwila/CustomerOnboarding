using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class EmailTemplateDto
    {
        public int Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string AssemblyQualifiedName { get; set; } = string.Empty;
    }
}
