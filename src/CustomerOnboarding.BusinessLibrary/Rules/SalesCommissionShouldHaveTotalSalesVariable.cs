using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class SalesCommissionShouldHaveTotalSalesVariable : BusinessRule
    {
        public Csla.Core.IPropertyInfo TypeProperty { get; private set; }
        public SalesCommissionShouldHaveTotalSalesVariable(
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
            if (!string.IsNullOrEmpty(formular) && type.HasValue)
            {
                if (type.Value == 3)
                {
                    var lexer = new MathGrammarLexer(new Antlr4.Runtime.AntlrInputStream(formular));
                    var tokens = lexer.GetAllTokens();
                    var hasTotalSalesVariables = false;
                    foreach (var token in tokens)
                    {
                        if (token.Text == "Total Sales")
                        {
                            hasTotalSalesVariables = true;
                            break;
                        }
                    }
                    if (!hasTotalSalesVariables)
                    {
                        context.AddErrorResult("Formular for Sales Commission  should contain Total Sales");
                    }
                }
            }
        }
    }
}
