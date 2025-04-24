using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class VariableTypeEntity
    {
        public int Id { get; set; }
       
        public string Pattern { get; set; } = string.Empty;
        
        public string TypeName { get; set; } = string.Empty;

        
        public byte[] LastChanged { get; set; } = default!;
    }
}
