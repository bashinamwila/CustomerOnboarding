using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class MarkItemAdditionStepsComplete :
        BusinessRule
    {
        public MarkItemAdditionStepsComplete(Csla.Core.IPropertyInfo
            primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }

#pragma warning disable CSLA0017
        protected override void Execute(IRuleContext context)
        {
            var steps = context.GetInputValue<Steps>(PrimaryProperty);
            var confirmationStep = steps.Where(r => r is IConfirmationAction)
                .Select(r => r).FirstOrDefault();
            var itemAdditionStep = steps.Where(r => r is IItemAdditionStep)
                .Select(r => r).FirstOrDefault();

            if(confirmationStep is not null && 
                itemAdditionStep is not null)
            {
                if(((IConfirmationAction)confirmationStep).ActionTaken
                    == ConfirmationActions.IHaveAddedAllTheCurrentItems)
                {
                    if (!itemAdditionStep.IsCompleted)
                    {
                        ((IItemAdditionStep)itemAdditionStep).MarkAsComplete();

                        LoadProperty(context.Target, PrimaryProperty, steps);
                    }
                }
            }
        }


#pragma warning restore CSLA0017

    }
}
