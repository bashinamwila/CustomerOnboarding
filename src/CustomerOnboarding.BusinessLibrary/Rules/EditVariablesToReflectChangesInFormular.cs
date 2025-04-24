using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.Types;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class EditVariablesToReflectChangesInFormular : BusinessRule
    {
        public Csla.Core.IPropertyInfo FormularProperty { get; private set; }

        public EditVariablesToReflectChangesInFormular(Csla.Core.IPropertyInfo
            primaryProperty, Csla.Core.IPropertyInfo formularProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[]{ primaryProperty, formularProperty });
            FormularProperty = formularProperty;
        }
        protected override void Execute(IRuleContext context)
        {
            var target = (IFormular)context.Target;
            if (!target.IsNew)
            {
                var variables = (Variables)context.InputPropertyValues[PrimaryProperty];
                var formular = context.InputPropertyValues[FormularProperty] as string;

                var lexer = new MathGrammarLexer(new Antlr4.Runtime.AntlrInputStream(formular));
                var dp = context.ApplicationContext.GetRequiredService<IDataPortal<VariableTypeTypeList>>();
                var tokenPatterns = dp.Fetch();

                var tokens = lexer.GetAllTokens();
                var ignoreList = new List<string> { "(", ")", "*", "+", "-", "/", "of" };
                var tempList = new List<IVariable>();
                foreach (var token in tokens)
                {
                    if (!ignoreList.Contains(token.Text))
                    {
                        var pattern = tokenPatterns.GetPattern(token.Text);
                        if (pattern != null)
                        {

                            if (variables.Contains(pattern.Id))
                            {
                                if (variables.GetItem(pattern.Id)!.Token != token.Text)
                                {
                                    variables.GetItem(pattern.Id)!.Token = token.Text;
                                    tempList.Add(variables.GetItem(pattern.Id)!);
                                }
                                else
                                {
                                    tempList.Add(variables.GetItem(pattern.Id)!);
                                }
                            }
                            if (!variables.Contains(pattern.Id))
                            {
                                variables.AddChild(pattern.Id, token.Text);
                                tempList.Add(variables.GetItem(pattern.Id)!);
                            }

                        }
                    }
                }
                if (tempList.Count != 0)
                {
                    for (int i = variables.Count - 1; i >= 0; i--)
                    {

                        if (!tempList.Contains(variables[i]))
                        {
                            variables.RemoveAt(i);
                        }
                    }
                }

                context.AddOutValue(PrimaryProperty, variables);
            }
        }
    }
}
