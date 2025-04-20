using Csla.Core;
using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SetOldEmployeeType : BusinessRule
    {

        public SetOldEmployeeType(
            Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);

        }
        protected override void Execute(IRuleContext context)
        {

            var target = (EmployeeEmploymentDetails)context.Target;
            var id = target.EmploymentType;
            if (!target.IsNew && target.EmployeeType?.Id != id)
            {
                var currentType = target.EmployeeType;
                var oldType = currentType;
                oldType?.DeleteChild();
                context.AddOutValue(PrimaryProperty, oldType);

            }



        }
    }
}
