using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class VerifyNRCFormat : BusinessRule
    {
        public VerifyNRCFormat(Csla.Core.IPropertyInfo
                primaryProperty)
                : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var target = (EmployeePersonalDetails)context.Target;
            var nrc = context.GetInputValue<string>(PrimaryProperty);
            if (target.IdType == 1)
            {
                if (!string.IsNullOrEmpty(nrc))
                {
                    var regex = new Regex(@"^\d{6}\/\d{2}\/\d{1}$");
                    if (!regex.IsMatch(nrc))
                    {
                        context.AddErrorResult("Invalid NRC Number");
                    }
                }
            }
        }
    }
}
