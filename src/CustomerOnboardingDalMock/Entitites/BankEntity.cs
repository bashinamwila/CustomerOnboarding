using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class BankEntity
    {
        public string BankId { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string SwiftCode { get; set; } = String.Empty;
        public byte[]? LastChanged { get; set; } = null;
    }
}
