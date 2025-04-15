using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class NoDuplicates : BusinessRule
    {
        public NoDuplicates(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var target = context.Target;
            var id = context.GetInputValue<string>(PrimaryProperty);
            var portal = context.ApplicationContext.GetRequiredService<IDataPortalFactory>();
            if (target is Wage wage)
            {
                var cmd = portal.GetPortal<WageExistsCommand>().Create(id);
                cmd = portal.GetPortal<WageExistsCommand>().Execute(cmd);
                if (cmd.Exists)
                    context.AddErrorResult($"Wage with {id} exists");
            }
            /*
            if (target is Deduction deduction)
            {
                var cmd = portal.GetPortal<DeductionExistsCommand>().Create(id);
                cmd = portal.GetPortal<DeductionExistsCommand>().Execute(cmd);
                if (cmd.Exists)
                    context.AddErrorResult($"Deduction with {id} exists");
            }
            if (target is EmployerExpense employerExpense)
            {
                var cmd = portal.GetPortal<EmployerExpenseExistsCommand>().Create(id);
                cmd = portal.GetPortal<EmployerExpenseExistsCommand>().Execute(cmd);
                if (cmd.Exists)
                    context.AddErrorResult($"Employer Expense with {id} exists");
            }
            */
        }
    }
}
