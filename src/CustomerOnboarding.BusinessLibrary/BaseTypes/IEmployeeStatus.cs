using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IEmployeeStatus : IBusinessBase
    {

        public int Id { get; }
        public string Name { get; }
    }
}
