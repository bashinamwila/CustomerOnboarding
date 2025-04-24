using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ValidWage : BusinessRule
    {
        public ValidWage(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var id = context.GetInputValue<string>(PrimaryProperty);
            var dp = context.ApplicationContext.GetRequiredService<IDataPortal<WageList>>();
            var tenantId = "123werqop070905mnbfghjkl";
            var list = dp.Fetch(tenantId);
            if (!list.Contains(id))
                context.AddErrorResult("Invalid Wage Id");
        }
    }
}
