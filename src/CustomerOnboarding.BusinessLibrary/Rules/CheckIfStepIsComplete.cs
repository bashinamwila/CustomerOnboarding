using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CheckIfStepIsComplete :BusinessRule
    {
        public CheckIfStepIsComplete(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty) :
            base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var target = (IStep)context.Target;
            var isComplete = target.IsValid;
            context.AddOutValue(AffectedProperties[1], isComplete);
        }
    }
}
