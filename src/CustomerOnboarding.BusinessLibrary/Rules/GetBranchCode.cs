using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class GetBranchCode : BusinessRuleAsync
    {
#pragma warning disable CSLA0017
        public Csla.Core.IPropertyInfo AlternativePrimaryProperty { get; private set; }
        public GetBranchCode(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo alternativePrimaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, alternativePrimaryProperty });
            AffectedProperties.Add(affectedProperty);
            AlternativePrimaryProperty = alternativePrimaryProperty;


        }
        protected override async Task ExecuteAsync(IRuleContext context)
        {
            string bankId = "";
            int branchId = 0;

            if (PrimaryProperty.Name == "BankId")
                bankId = context.GetInputValue<string>(PrimaryProperty);
            else
                if (AlternativePrimaryProperty.Name == "BankId")
                bankId = context.GetInputValue<string>(AlternativePrimaryProperty);

            if (PrimaryProperty.Name == "BranchId")
                branchId = context.GetInputValue<int>(PrimaryProperty);
            else
                if (AlternativePrimaryProperty.Name == "BranchId")
                branchId = context.GetInputValue<int>(AlternativePrimaryProperty);
            var bankList = await context.DataPortalFactory.GetPortal<BankList>().FetchAsync();
            var bank = (from r in bankList
                        where r.Id == bankId
                        select r).FirstOrDefault();
            if (bank != null)
            {
                var branch = bank.Branches.GetItem(branchId);
                if (branch != null)
                    context.AddOutValue(AffectedProperties[1], branch.BranchCode);
#pragma warning restore CSLA0017
            }
        }
    }
}
