using Csla;
using Csla.Rules;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SetEmployeeStatus :
        BusinessRule
    {
        public SetEmployeeStatus(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var id = context.GetInputValue<int?>(PrimaryProperty);
            if (id.HasValue)
            {
                var portal = context.ApplicationContext.GetRequiredService<IDataPortal<EmployeeStatusFactory>>();
                var target = (EmployeeEmploymentDetails)context.Target;
                if(target.Status is not null)
                {
                    if (target.Status.Id != id)
                    {
                       
                        var status = portal.Fetch(id.Value, ((AddEmployeeEmploymentDetailsStep)target.Parent).RuleSet).Result;
                        context.AddOutValue(AffectedProperties[1], status);
                    }
                }
                else
                {
                    var status = portal.Fetch(id.Value, ((AddEmployeeEmploymentDetailsStep)target.Parent).RuleSet).Result;
                    context.AddOutValue(AffectedProperties[1], status);
                }
            }
        }
    }
}
