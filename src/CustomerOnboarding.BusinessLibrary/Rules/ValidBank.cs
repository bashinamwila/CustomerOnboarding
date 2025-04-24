using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ValidBank : BusinessRule
    {
        public ValidBank(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var id = context.GetInputValue<string>(PrimaryProperty);
            var bankList = context.DataPortalFactory.GetPortal<BankList>().Fetch();
            var bank = (from r in bankList
                        where r.Id == id
                        select r).FirstOrDefault();
            if (bank == null)
                context.AddErrorResult("Invalid Bank Id");
        }
    }
}
