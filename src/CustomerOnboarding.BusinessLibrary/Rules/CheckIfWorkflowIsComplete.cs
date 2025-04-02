using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CheckIfWorkflowIsComplete :
        BusinessRule
    {
        public CheckIfWorkflowIsComplete(Csla.Core.IPropertyInfo primaryProperty,Csla.Core.IPropertyInfo affectedProperty) :
            base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var steps = ((UserOnboardingOrchestrator)context.Target).Steps;
            var isComplete = steps.All(s => s.IsCompleted);
            context.AddOutValue(AffectedProperties[1],isComplete);
        }
    }
}
