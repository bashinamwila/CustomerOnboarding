using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SetEmployerExpenseType : BusinessRule
    {
        public SetEmployerExpenseType(Csla.Core.IPropertyInfo primaryProperty,
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
                var target = (EmployerExpense)context.Target;
                var parent = (AddEmployerExpenseStep)target.Parent;
                if (parent != null)
                {
                    var dp = context.ApplicationContext.GetRequiredService<IDataPortal<EmployerExpenseTypeFactory>>();
                    var type = dp.Fetch(id, parent.RuleSet).Result;
                    context.AddOutValue(AffectedProperties[1], type);
                }

            }
        }
    }
}
