using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IEarning
    {
        string Id { get; set; }
        string Name { get; set; }
        int? Type { get; }
    }
}
