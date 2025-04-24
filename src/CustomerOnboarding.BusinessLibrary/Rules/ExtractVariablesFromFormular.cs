using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.Types;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class ExtractVariablesFromFormular : BusinessRule
    {
        public Csla.Core.IPropertyInfo FormularProperty { get; private set; }

        public ExtractVariablesFromFormular(Csla.Core.IPropertyInfo
            primaryProperty, Csla.Core.IPropertyInfo formularProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new []{ primaryProperty, formularProperty });
            FormularProperty = formularProperty;
        }

        protected override void Execute(IRuleContext context)
        {
            var target = (IFormular)context.Target;

            var variables = context.GetInputValue<Variables>(PrimaryProperty);
            var formular = context.GetInputValue<string>(FormularProperty);
            //var logger = context.ApplicationContext.GetRequiredService<ILogger<ExtractVariablesFromFormular>>();
            if (!string.IsNullOrEmpty(formular))
            {
                var lexer = new MathGrammarLexer(new Antlr4.Runtime.AntlrInputStream(formular));
                var dp = context.ApplicationContext.GetRequiredService<IDataPortal<VariableTypeTypeList>>();
                var tokenPatterns = dp.Fetch();
                var tokens = lexer.GetAllTokens();
                var ignoreList = new List<string> { "(", ")", "*", "+", "-", "/", "of" };
                if (target.IsNew)
                {
                    foreach (var token in tokens)
                    {
                      //  logger.LogInformation($"Evaluating token {token.Text}");
                        if (!ignoreList.Contains(token.Text))
                        {
                            var pattern = tokenPatterns.GetPattern(token.Text.Trim());
                            if (pattern != null)
                            {

                                variables.AddChild(pattern.Id, token.Text);

                            }
                        }
                        context.AddOutValue(PrimaryProperty, variables);
                    }
                }
            }
        }
    }
}
