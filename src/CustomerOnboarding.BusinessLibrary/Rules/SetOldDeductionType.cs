using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SetOldDeductionType : BusinessRule
    {
        public SetOldDeductionType(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
        : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var target = (Deduction)context.Target;
            var type = context.GetInputValue<int>(PrimaryProperty);
            if (!target.IsNew)
            {
                if (target.DeductionType.Id != type)
                {
                    var deductionType = target.DeductionType;
                    deductionType.DeleteChild();
                    context.AddOutValue(AffectedProperties[1], deductionType);
                }
            }
        }
    }
}
