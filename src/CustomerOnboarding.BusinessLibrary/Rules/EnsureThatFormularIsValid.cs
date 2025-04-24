using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class EnsureThatFormularIsValid : BusinessRule
    {
        public EnsureThatFormularIsValid(
            Csla.Core.IPropertyInfo primaryProperty)
        : base(primaryProperty)
        {
            InputProperties.Add( primaryProperty );


        }
        protected override void Execute(IRuleContext context)
        {

            var formular = context.GetInputValue<string>(PrimaryProperty);

            if (!string.IsNullOrEmpty(formular))
            {
                var lexer = new MathGrammarLexer(new Antlr4.Runtime.AntlrInputStream(formular));
                var ignoreList = new List<string> { "(", ")", "*", "+", "-", "/", "of" };
                var tokens = lexer.GetAllTokens();

                if (tokens.Count > 0)
                {
                    if (tokens.Count == 1)
                    {
                        if (ignoreList.Contains(tokens[0].Text))
                            context.AddErrorResult("Formular is not valid");
                    }
                    else
                    {
                        if (!HasBrackets(tokens))
                        {
                            if (tokens.Count % 2 != 0)
                            {

                                for (var i = 0; i <= tokens.Count - 1; i++)
                                {
                                    if ((i + 1) % 2 == 0)
                                    {
                                        var prevToken = tokens[i - 1];
                                        var nextToken = tokens[i + 1];
                                        if (!ignoreList.Contains(tokens[i].Text))
                                            context.AddErrorResult("Formular is not valid");
                                        if (ignoreList.Contains(prevToken.Text) || ignoreList.Contains(nextToken.Text))
                                            context.AddErrorResult("Formular is not valid");

                                    }
                                }
                            }
                            else
                            {
                                context.AddErrorResult("Formular is not valid");
                            }
                        }
                        else
                        {

                            if (OpeningBracketsMatchClosingBrackets(tokens))
                            {
                                tokens = StripBrackets(tokens);
                                if (tokens.Count == 1)
                                {
                                    if (ignoreList.Contains(tokens[0].Text))
                                        context.AddErrorResult("Formular is not valid");
                                }
                                else
                                {
                                    if (tokens.Count % 2 != 0)
                                    {

                                        for (var i = 0; i <= tokens.Count - 1; i++)
                                        {
                                            if ((i + 1) % 2 == 0)
                                            {
                                                var prevToken = tokens[i - 1];
                                                var nextToken = tokens[i + 1];
                                                if (!ignoreList.Contains(tokens[i].Text))
                                                    context.AddErrorResult("Formular is not valid");
                                                if (ignoreList.Contains(prevToken.Text) || ignoreList.Contains(nextToken.Text))
                                                    context.AddErrorResult("Formular is not valid");

                                            }
                                        }
                                    }
                                    else
                                    {
                                        context.AddErrorResult("Formular is not valid");
                                    }
                                }

                            }
                            else
                            {
                                context.AddErrorResult("Formular is not valid");
                            }
                        }



                    }
                }
                else
                {
                    context.AddErrorResult("Formular is not valid");
                }



            }



        }

        private bool HasBrackets(IList<Antlr4.Runtime.IToken> tokens)
        {
            bool hasBrackets = false;
            foreach (var token in tokens)
            {
                if (token.Text == ")" || token.Text == "(")
                {
                    hasBrackets = true;
                    break;
                }
            }
            return hasBrackets;

        }
        private bool OpeningBracketsMatchClosingBrackets(IList<Antlr4.Runtime.IToken> tokens)
        {

            int openBrackets = 0;
            int closingBrackets = 0;
            foreach (var token in tokens)
            {
                if (token.Text == ")")
                {
                    openBrackets += 1;
                }
                else
                {
                    if (token.Text == "(")
                    {
                        closingBrackets += 1;
                    }
                }
            }
            return openBrackets == closingBrackets;
        }
        private IList<Antlr4.Runtime.IToken> StripBrackets(IList<Antlr4.Runtime.IToken> tokens)
        {

            for (var i = tokens.Count - 1; i >= 0; i--)
            {
                if (tokens[i].Text == "(" || tokens[i].Text == ")")
                {
                    tokens.RemoveAt(i);
                }

            }
            return tokens;
        }

    }
}
