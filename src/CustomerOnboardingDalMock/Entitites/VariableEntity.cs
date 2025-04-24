using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class VariableEntity
    {

        public int Id { get; set; }

        public string ItemId { get; set; } = string.Empty;

        public string TenantId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;


        public decimal? Value { get; set; } = default!;


        public byte[] LastChanged { get; set; } = default!;




    }
}
