using Csla;
using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SetEmployeeDataInputMethod :
        BusinessRule
    {
        public SetEmployeeDataInputMethod(Csla.Core.IPropertyInfo
            primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty});
            
        }
#pragma warning disable CSLA0017
        protected override void Execute(IRuleContext context)
        {
            var method = context.GetInputValue<int>(PrimaryProperty);
            if (method > 0)
            {
                var target = (GeneralEmployeesInformationStep)context.Target;
          
                var parent = target.Parent.Parent as EmployeesStep;

                if (parent is not null)
                {
                    
                    var portal = context.ApplicationContext.GetRequiredService<IDataPortal<EmployeeDataInputMethodFactory>>();
                    var factory = portal.Fetch(method,parent.CurrentStepIndex);
                   // var steps = context.GetInputValue<Steps>(AffectedProperties[1]);
                    parent.Steps.Add(factory.Result);
                    LoadProperty(parent, EmployeesStep.StepsProperty, parent.Steps);
                }
            }
#pragma warning restore CSLA0017
        }
    }
}
