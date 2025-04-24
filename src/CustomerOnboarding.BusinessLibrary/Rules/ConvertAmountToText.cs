using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ConvertAmountToText : BusinessRule
    {
        public ConvertAmountToText(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var amount = context.GetInputValue<decimal>(PrimaryProperty);
            var amountToText = amount.ToString("#,##0.00");
            context.AddOutValue(AffectedProperties[1], amountToText);
        }
    }
}
