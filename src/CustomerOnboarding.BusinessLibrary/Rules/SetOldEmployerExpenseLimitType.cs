using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SetOldEmployerExpenseLimitType : BusinessRule
    {
        public SetOldEmployerExpenseLimitType(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var target = (EmployerExpense)context.Target;
            var type = context.GetInputValue<int>(PrimaryProperty);
            if (!target.IsNew)
            {
                if (target.EmployerExpenseLimitType.Id != type)
                {
                    var EmployerExpenseLimitType = target.EmployerExpenseLimitType;
                    EmployerExpenseLimitType.DeleteChild();
                    context.AddOutValue(AffectedProperties[1], EmployerExpenseLimitType);
                }
            }

        }
    }
}
