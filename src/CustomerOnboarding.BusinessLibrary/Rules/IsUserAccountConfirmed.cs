using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class IsUserAccountConfirmed : BusinessRule
    {
        public IsUserAccountConfirmed(Csla.Core.IPropertyInfo primaryProperty) :
            base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var isConfirmed = (bool)context.InputPropertyValues[PrimaryProperty];
            if (!isConfirmed)
            {
                context.AddErrorResult("User account is not confirmed.");
            }
        }
    }
}
