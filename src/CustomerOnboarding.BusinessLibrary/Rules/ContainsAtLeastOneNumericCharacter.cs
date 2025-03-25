using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ContainsAtLeastOneNumericCharacter : BusinessRule
    {
        public ContainsAtLeastOneNumericCharacter(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var password = context.GetInputValue<string>(PrimaryProperty);
            if (!password.Any(char.IsDigit))
            {
                context.AddErrorResult("Password must contain at least one numeric character");
            }
        }
    }

}
