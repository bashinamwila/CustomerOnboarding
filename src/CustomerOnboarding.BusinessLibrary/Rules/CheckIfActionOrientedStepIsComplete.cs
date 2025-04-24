using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CheckIfActionOrientedStepIsComplete :
        BusinessRule
    {

        public CheckIfActionOrientedStepIsComplete(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var action = context.GetInputValue<ConfirmationActions>(PrimaryProperty);
            if(action==ConfirmationActions.IHaveAddedAllTheCurrentItems)
            {
                context.AddOutValue(AffectedProperties[1], true);
            }
        }
    }
}
