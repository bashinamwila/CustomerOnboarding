using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class EmployeeDataInputMethodTypeEntity
    {
        public int Id { get; set; }
        public string FullTypeName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string FriendlyName { get; set; } = string.Empty;

        public string ComponentTypeName { get; set; } = string.Empty;

        public byte[] LastChanged { get; set; } = default!;
    }
}
