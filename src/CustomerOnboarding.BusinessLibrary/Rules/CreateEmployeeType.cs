using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CreateEmployeeType : BusinessRule
    {
        public CreateEmployeeType(Csla.Core.IPropertyInfo primaryProperty) : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var target = (EmployeeEmploymentDetails)context.Target;

            var id = target.EmploymentType;
            if (id.HasValue)
            {
                if (id.Value != 0)
                {
                    if (target.EmployeeType == null || target.EmployeeType.Id != id)
                    {
                        IEmployeeType type = context.DataPortalFactory.GetPortal<EmployeeTypeFactory>().Fetch(id.Value).Result;
                        context.AddOutValue(PrimaryProperty, type);
                    }

                }
            }
        }
    }
}
