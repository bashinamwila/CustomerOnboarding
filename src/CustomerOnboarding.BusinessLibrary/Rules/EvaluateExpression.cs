using Antlr4.Runtime;
using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public abstract class EvaluateExpression : BusinessRule
    {

        public Csla.Core.IPropertyInfo FormularProperty { get; private set; }
        public Csla.Core.IPropertyInfo VariablesProperty { get; private set; }
        public EvaluateExpression(Csla.Core.IPropertyInfo
            primaryProperty,
            Csla.Core.IPropertyInfo formularProperty,
            Csla.Core.IPropertyInfo variablesProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, formularProperty, variablesProperty });
            FormularProperty = formularProperty;
            VariablesProperty = variablesProperty;

        }

        public decimal Calculate(string expression, IDictionary<string, decimal> variables)
        {
            var errorListener = new CustomErrorListener<int>();

            var inputStream = new AntlrInputStream(expression);
            var lexer = new MathGrammarLexer(inputStream);
            lexer.RemoveErrorListeners(); // remove the default error listener
            lexer.AddErrorListener(errorListener); // add our custom error listener

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new MathGrammarParser(commonTokenStream);
            parser.RemoveErrorListeners(); // remove the default error listener
            parser.AddErrorListener(new CustomErrorListener<IToken>()); // add our custom error listener

            try
            {
                var tree = parser.start(); // Parse the expression

                var visitor = new MathGrammarVisitor(variables);
                return visitor.Visit(tree); // Evaluate the expression
            }
            catch (Exception ex)
            {
                // Handle exceptions that occur during parsing or evaluation
                throw new Exception($"Error evaluating expression: {ex.Message}", ex);
            }
        }

        // Custom error listener that throws an exception when a syntax error occurs

    }
}
