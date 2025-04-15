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
                var isComplete = target.Steps
                    .Where(step => step is not IItemAdditionStep)
                    .All(step => step.IsCompleted);

                context.AddOutValue(AffectedProperties[1], isComplete);
            }
        }
    }
}
