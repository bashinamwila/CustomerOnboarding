using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class MinimumLimitGTMaximumLimit : BusinessRule
    {
        public Csla.Core.IPropertyInfo AffectedProperty { get; private set; }

        public MinimumLimitGTMaximumLimit(Csla.Core.IPropertyInfo
            primaryProperty, Csla.Core.IPropertyInfo affectedProperty)
        : base(primaryProperty)
        {
            InputProperties.AddRange(new[]{ primaryProperty, affectedProperty });
            this.AffectedProperty = affectedProperty;
        }
        protected override void Execute(IRuleContext context)
        {
            var target = (ILimit)context.Target;
            if (target.Minimum > target.Maximum)
                context.AddErrorResult("The minimum limit can not be greater than the maximum limit");
        }
    }
}
