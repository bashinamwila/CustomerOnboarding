using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class GetBankName : BusinessRule
    {
        public GetBankName(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var id = context.GetInputValue<string>(AffectedProperties[1]);
            var portal = context.ApplicationContext.GetRequiredService<IDataPortal<BankList>>();
            var bankList = portal.Fetch();
            var bank = (from r in bankList
                        where r.Id == id
                        select r).FirstOrDefault();
            context.AddOutValue(PrimaryProperty, bank?.Name);
        }

    }
}
