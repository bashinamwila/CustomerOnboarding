using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class StepIsCompletedIfValidWageIsSelected :
        BusinessRule
    {
        public StepIsCompletedIfValidWageIsSelected(Csla.Core.IPropertyInfo
            primaryProperty,Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var portal = context.DataPortalFactory.GetPortal<WageExistsCommand>();
            var id = context.GetInputValue<string>(PrimaryProperty);
            var cmd = portal.Execute(id);
            if (!string.IsNullOrEmpty(id))
            {
                if (cmd.Exists)
                {
                    context.AddOutValue(AffectedProperties[1], true);
                }
            }
        }
    }
}
