using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IFormular : IBusinessBase
    {
        string Formular { get; set; }
        //Benefito.BusinessLibrary.Admin.Variables Variables { get; }

    }
}
