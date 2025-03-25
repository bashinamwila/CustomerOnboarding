using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IFullName : IBusinessBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
