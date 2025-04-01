using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IEmailTemplate :IBusinessBase
    {
        public string Template { get; }
        public string TemplateName { get; }
    }
}
