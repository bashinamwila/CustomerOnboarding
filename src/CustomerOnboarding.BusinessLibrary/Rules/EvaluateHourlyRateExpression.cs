using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class EvaluateHourlyRateExpression : EvaluateExpression
    {
#pragma warning disable CSLA0017 // Find Business Rules That Do Not Use Add() Methods on the Context

        public EvaluateHourlyRateExpression(Csla.Core.IPropertyInfo
            primaryProperty,
            Csla.Core.IPropertyInfo formularProperty,
            Csla.Core.IPropertyInfo variablesProperty)
            : base(primaryProperty, formularProperty, variablesProperty)
        { }

        protected override void Execute(IRuleContext context)
        {
            var formular = context.GetInputValue<string>(FormularProperty);
            var target = context.Target;
            if (target is EmployeeHourlyRateWage)
            {
                var variables = context.GetInputValue<EmployeeVariables>(VariablesProperty);
                var variableList = new Dictionary<string, decimal>();



                var doesContainVariableLateral = false;
                var token = string.Empty;

                var totalHrsWorkedVariable = (from r in variables
                                              where r.Id == 19
                                              select r).FirstOrDefault();
                if (totalHrsWorkedVariable != null)
                {
                    totalHrsWorkedVariable.Value = 100m; //dummy value
                    token = totalHrsWorkedVariable.Token;
                }

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
                    LoadProperty(context.Target, PrimaryProperty, amount / variableList[token]);
                }
                else
                {
                    if (target is EmployeeHourlyRateWageInfo)
                    {
                        var _variables = context.GetInputValue<EmployeeVariableList>(VariablesProperty);
                        var _variableList = new Dictionary<string, decimal>();



                        var _doesContainVariableLateral = false;




                        foreach (var variable in _variables)
                        {
                            if (!variable.Value.HasValue && variable is not EmployeeTotalHoursWorkedVariableInfo)
                            {
                                _doesContainVariableLateral = true;
                            }

                        }
                        if (!_doesContainVariableLateral)
                        {

                            foreach (var variable in _variables)
                            {
                                if (variable is EmployeePercentVariableInfo)
                                {
                                    _variableList[variable.Token] = variable.Value!.Value / 100;
                                }
                                else
                                {
                                    if (variable is EmployeeTotalHoursWorkedVariableInfo)
                                    {
                                        _variableList[variable.Token] = 100m;
                                    }
                                    else
                                    {
                                        _variableList[variable.Token] = variable.Value!.Value;
                                    }
                                }

                            }
                            var amount = Calculate(context.GetInputValue<string>(FormularProperty), _variableList);
                            LoadProperty(context.Target, PrimaryProperty, amount / _variableList[token]);
                        }
                    }
                }
            }






#pragma warning restore CSLA0017 // Find Business Rules That Do Not Use Add() Methods on the Context
        }


    }
}
