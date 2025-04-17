using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SetDeductionType : BusinessRule
    {
        public SetDeductionType(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }
        protected override void Execute(IRuleContext context)
        {

            var id = context.GetInputValue<int>(PrimaryProperty);
            if (id > 0)
            {
                var target = (Deduction)context.Target;
                var parent=(AddDeductionStep)target.Parent;
                if(parent != null)
                {
                    var dp = context.ApplicationContext.GetRequiredService<IDataPortal<DeductionTypeFactory>>();
                    var type = dp.Fetch(id,parent.RuleSet).Result;
                    context.AddOutValue(AffectedProperties[1], type);
                }
               
            }
        }
    }
}
