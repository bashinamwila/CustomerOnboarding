using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class HourlyWageShouldContainAtLeastMultiplierAndTotalHoursWorkedVariables
        : BusinessRule
    {
        public Csla.Core.IPropertyInfo TypeProperty { get; private set; }
        public HourlyWageShouldContainAtLeastMultiplierAndTotalHoursWorkedVariables(
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
            var hasTotalHours = false;
            if (!string.IsNullOrEmpty(formular) && type.HasValue)
            {
                if (type.Value == 2)
                {
                    var lexer = new MathGrammarLexer(new Antlr4.Runtime.AntlrInputStream(formular));
                    var tokens = lexer.GetAllTokens();
                    foreach (var token in tokens)
                    {
                        if (token.Text == "Multiplier")
                        {
                            hasMultiplier = true;
                        }
                        if (token.Text == "Total Hours Worked")
                        {
                            hasTotalHours = true;
                        }
                    }
                    if (!hasTotalHours && !hasMultiplier)
                    {
                        context.AddErrorResult("Formular for Hourly Wage should contain both Multiplier & Total Hours Worked");
                    }
                    else if (hasMultiplier && !hasTotalHours)
                    {
                        context.AddErrorResult("Formular for Hourly Wage should contain Total Hours Worked");
                    }
                    else
                    {
                        if (hasTotalHours && !hasMultiplier)
                        {
                            context.AddErrorResult("Formular for Hourly Wage should contain Multiplier");
                        }
                    }

                }

            }
        }
    }
}
