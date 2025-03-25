using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ValidateEmailAddress : BusinessRule
    {
        public ValidateEmailAddress(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var email = context.GetInputValue<string>(PrimaryProperty);
            if (string.IsNullOrEmpty(email)) return;

            var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!regex.IsMatch(email))
            {
                context.AddErrorResult($"Invalid email address");
            }
        }
    }
}
