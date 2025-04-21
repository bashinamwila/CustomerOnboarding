using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class EvaluateAmountExpression : EvaluateExpression
    {
#pragma warning disable CSLA0017 // Find Business Rules That Do Not Use Add() Methods on the Context

        public EvaluateAmountExpression(Csla.Core.IPropertyInfo
            primaryProperty,
            Csla.Core.IPropertyInfo formularProperty,
            Csla.Core.IPropertyInfo variablesProperty)
            : base(primaryProperty, formularProperty, variablesProperty)
        { }

        protected override void Execute(IRuleContext context)
        {
            var formular = context.GetInputValue<string>(FormularProperty);
            var variables = context.GetInputValue<EmployeeVariables>(VariablesProperty);
            var variableList = new Dictionary<string, decimal>();



            var doesContainVariableLateral = false;



            foreach (var variable in variables)
            {
                if (!variable.Value.HasValue)
                {
                    doesContainVariableLateral = true;
                }
            }
            if (!doesContainVariableLateral)
            {

                foreach (var variable in variables)
                {
                    if (variable is EmployeePercentVariable)
                    {
                        variableList[variable.Token] = variable.Value!.Value / 100;
                    }
                    else
                    {
                        variableList[variable.Token] = variable.Value!.Value;
                    }

                }
                var amount = Calculate(context.GetInputValue<string>(FormularProperty), variableList);
                LoadProperty(context.Target, PrimaryProperty, amount);
            }





#pragma warning restore CSLA0017 // Find Business Rules That Do Not Use Add() Methods on the Context
        }




    }
}
