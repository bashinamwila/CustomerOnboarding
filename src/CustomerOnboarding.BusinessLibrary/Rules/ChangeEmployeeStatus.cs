using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ChangeEmployeeStatus : BusinessRule
    {
        public ChangeEmployeeStatus(Csla.Core.IPropertyInfo primaryProperty) : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var target = (EmployeeEmploymentDetails)context.Target;

            var id = target.StatusId;
            if (id.HasValue)
            {
                if (id.Value != 0)
                {
                    if (target.IsNew)
                    {
                        IEmployeeStatus status = context.DataPortalFactory.GetPortal<EmployeeStatusFactory>().Fetch(id.Value).Result;
                        context.AddOutValue(PrimaryProperty, status);
                    }
                    else if (target.Status.Id != id)
                    {
                        IEmployeeStatus status = context.DataPortalFactory.GetPortal<EmployeeStatusFactory>().Fetch(id.Value).Result;
                        context.AddOutValue(PrimaryProperty, status);
                    }

                }
            }
        }
    }
}
