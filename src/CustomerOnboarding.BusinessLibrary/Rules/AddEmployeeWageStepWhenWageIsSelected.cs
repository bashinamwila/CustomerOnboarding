using Csla;
using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class AddEmployeeWageStepWhenWageIsSelected :
        BusinessRule
    {

        public AddEmployeeWageStepWhenWageIsSelected(Csla.Core.IPropertyInfo
            primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
#pragma warning disable CSLA0017

        protected override void Execute(IRuleContext context)
        {
            var id = context.GetInputValue<string>(PrimaryProperty);
            var portal = context.ApplicationContext.GetRequiredService<IDataPortalFactory>();
            var cmd = portal.GetPortal<WageExistsCommand>().Execute(id);

            if(!string.IsNullOrEmpty(id) && cmd.Exists)
            {
                var wageInfo = portal.GetPortal<WageInfo>().Fetch(id);
                var childPortal = context.ApplicationContext.GetRequiredService<IChildDataPortal<AddEmployeeWageStep>>();
                var target = (SelectEmployeeWageStep)context.Target;
                var parent = (EmployeeWagesStep)target.Parent.Parent;
                var step = childPortal.CreateChild(wageInfo, parent.CurrentStepIndex);
                parent.Steps.Add(step);
                LoadProperty(parent, EmployeeWagesStep.StepsProperty, parent.Steps);
            }
#pragma warning restore CSLA0017
        }
    }
}
