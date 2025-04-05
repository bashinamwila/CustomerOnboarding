using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ValidBankBranch : BusinessRuleAsync
    {
        public ValidBankBranch(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }
        protected override async Task ExecuteAsync(IRuleContext context)
        {
            var id = context.GetInputValue<string>(AffectedProperties[1]);
            var branchId = context.GetInputValue<int>(PrimaryProperty);
            var bankList = await context.DataPortalFactory.GetPortal<BankList>().FetchAsync();
            var bank = (from r in bankList
                        where r.Id == id
                        select r).FirstOrDefault();
            if (bank != null)
            {
                var branch = bank.Branches.GetItem(branchId);
                if (branch == null)
                    context.AddErrorResult("Invalid Branch Id");
            }

        }
    }
}
