using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ConvertSalesCommissionFormularToText : BusinessRule
    {
        public Csla.Core.IPropertyInfo FormularProperty { get; private set; }
        public Csla.Core.IPropertyInfo VariablesProperty { get; private set; }
        public ConvertSalesCommissionFormularToText(Csla.Core.IPropertyInfo primaryProperty,
           Csla.Core.IPropertyInfo formularProperty,
           Csla.Core.IPropertyInfo variablesProperty)
           : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, formularProperty, variablesProperty });
            VariablesProperty = variablesProperty;
            FormularProperty = formularProperty;
        }
        protected override void Execute(IRuleContext context)
        {
            var formular = context.GetInputValue<string>(FormularProperty);
            var variables = context.GetInputValue<EmployeeVariables>(VariablesProperty);
            var operatorsAndBrackets = new List<string> { "(", ")", "*", "+", "-", "/", "of" };
            if (!string.IsNullOrEmpty(formular))
            {
                var lexer = new MathGrammarLexer(new Antlr4.Runtime.AntlrInputStream(formular));
                var tokens = lexer.GetAllTokens();
                foreach (var token in tokens)
                {
                    if (operatorsAndBrackets.Contains(token.Text))
                    {
                        switch (token.Text)
                        {
                            case "*":
                                formular = formular.Replace("*", "of");
                                break;
                            case "-":
                                formular = formular.Replace("-", "minus");
                                break;
                            case "+":
                                formular = formular.Replace("+", "plus");
                                break;
                            case "/":
                                formular = formular.Replace("/", "divide by");
                                break;
                        }
                    }
                    else
                    {
                        var variable = (from r in variables
                                        where r.Token == token.Text
                                        select r).FirstOrDefault();
                        if (variable != null)
                        {
                            if (variable.Value.HasValue)
                            {
                                if (variable is EmployeePercentVariable)
                                {
                                    formular = formular.Replace(token.Text, $"{variable.Value.Value}%");
                                }
                                else
                                {
                                    formular = formular.Replace(token.Text, variable.Value.Value.ToString("#,##0.00"));
                                }
                            }
                        }
                    }
                }
                context.AddOutValue(PrimaryProperty, formular);
            }
        }
    }
}
