using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class GetFullName : Csla.Rules.BusinessRule
    {
        protected override void Execute(IRuleContext context)
        {
            var target = (ITypeInfo)context.Target;
            var assemblyQualifiedName = target.FullTypeName;
            var indexOfComma = assemblyQualifiedName.IndexOf(",");
            var fullName = assemblyQualifiedName?.Substring(0, indexOfComma);
            context.AddOutValue(PrimaryProperty, fullName);
        }
    }
}
