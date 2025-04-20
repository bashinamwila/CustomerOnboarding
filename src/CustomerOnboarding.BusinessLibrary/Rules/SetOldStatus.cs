using Csla.Core;
using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SetOldStatus : BusinessRule
    {

        public SetOldStatus(
            Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);

        }
        protected override void Execute(IRuleContext context)
        {

            var target = (EmployeeEmploymentDetails)context.Target;
            var id = target.StatusId;
            if (!target.IsNew && target.Status?.Id != id)
            {
                var currentType = target.Status;
                var oldType = currentType;
                oldType?.DeleteChild();
                context.AddOutValue(PrimaryProperty, oldType);

            }



        }
    }
}
