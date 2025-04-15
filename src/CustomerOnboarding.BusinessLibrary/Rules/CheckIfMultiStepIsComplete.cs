using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CheckIfMultiStepIsComplete :
        BusinessRule
    {
        public CheckIfMultiStepIsComplete(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var target = context.Target as IOnboardingOrchestrator;
            if (target is not null)
            {
                //steps which implement the IItemAdditionStep interface
                //are not considered complete unless the item has all it's 
                //properties set.But in the event the item addition multi step
                // is complete the item will not have it's properties set
                //thereby rendering the step incomplete.
                var isComplete = target.Steps
                    .Where(step => step is not IItemAdditionStep)
                    .All(step => step.IsCompleted);

                context.AddOutValue(AffectedProperties[1], isComplete);
            }
        }
    }
}
