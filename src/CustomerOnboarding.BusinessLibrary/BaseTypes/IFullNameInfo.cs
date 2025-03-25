using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IFullNameInfo
    {
        public string FirstName { get; }
        public string LastName { get; }
    }
}
