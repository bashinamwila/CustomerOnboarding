using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CheckIfPasswordsMatch :
        BusinessRule
    {
        public CheckIfPasswordsMatch(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var password1 = context.GetInputValue<string>(PrimaryProperty);
            var password2 = context.GetInputValue<string>(AffectedProperties[1]);
            if (!string.IsNullOrWhiteSpace(password1) &&
                !string.IsNullOrWhiteSpace(password2)
                && !string.Equals(password1, password2))
            {
                context.AddErrorResult("Passwords do not match");
            }
        }
    }
}
