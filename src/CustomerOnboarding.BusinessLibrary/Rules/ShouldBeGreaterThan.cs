using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ShouldBeGreaterThan : BusinessRule
    {
        public decimal Threshold { get; private set; }
        public ShouldBeGreaterThan(Csla.Core.IPropertyInfo primaryProperty, decimal threshold)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
            RuleUri.AddQueryParameter("Threshold", threshold.ToString());
            Threshold = threshold;

        }
        protected override void Execute(IRuleContext context)
        {
            var value = context.GetInputValue<decimal>(PrimaryProperty);
            if (value <= Threshold)
                context.AddErrorResult($"Value should be greater than {Threshold.ToString("#,##0.00")}");
        }
    }
}
