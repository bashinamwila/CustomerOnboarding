using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ContainsAtLeastOneLowerCaseCharacter : BusinessRule
    {
        public ContainsAtLeastOneLowerCaseCharacter(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        override protected void Execute(Csla.Rules.IRuleContext context)
        {
            var password = context.GetInputValue<string>(PrimaryProperty);
            if (!password.Any(char.IsLower))
            {
                context.AddErrorResult("Password must contain at least one lower case character");
            }
        }
    }
}
