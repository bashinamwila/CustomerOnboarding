using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class GetTypeName : Csla.Rules.BusinessRule
    {
        protected override void Execute(IRuleContext context)
        {
            var target = (ITypeInfo)context.Target;
            var fullName = target.FullName;
            var lastIndexOf = fullName.LastIndexOf(".");
            var name = fullName.Substring(lastIndexOf + 1);
            context.AddOutValue(PrimaryProperty, name);
        }
    }
}
