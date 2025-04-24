using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SalaryCannotContainMultiplierTotalSalesAndTotalHoursWorkedVariables
         : BusinessRule
    {
        public Csla.Core.IPropertyInfo TypeProperty { get; private set; }
        public SalaryCannotContainMultiplierTotalSalesAndTotalHoursWorkedVariables(
            Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo typeProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, typeProperty });
            TypeProperty = typeProperty;
        }
        protected override void Execute(IRuleContext context)
        {
            var formular = context.GetInputValue<string>(PrimaryProperty);
            var type = context.GetInputValue<int?>(TypeProperty);
            var hasMultiplier = false;
            var hasTotalHoursWorked = false;
            var hasTotalSales = false;
            if (!string.IsNullOrEmpty(formular) && type.HasValue)
            {
                if (type.Value == 1)
                {
                    var lexer = new MathGrammarLexer(new Antlr4.Runtime.AntlrInputStream(formular));
                    var tokens = lexer.GetAllTokens();
                    foreach (var token in tokens)
                    {
                        if (token.Text == "Multiplier")
                        {
                            hasMultiplier = true;
                        }
                        if (token.Text == "Total Sales")
                        {
                            hasTotalSales = true;
                        }
                        if (token.Text == "Total Hours Worked")
                        {
                            hasTotalHoursWorked = true;
                        }
                    }
                    if (hasMultiplier || hasTotalHoursWorked || hasTotalSales)
                    {
                        context.AddErrorResult("Formular for Salary cannot either Total Sales,Total Hours Worked or Multiplier");
                    }
                }
            }
        }
    }
}
