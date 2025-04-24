using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Parsing
{
    public class MathGrammarVisitor : MathGrammarBaseVisitor<decimal>
    {
        private readonly IDictionary<string, decimal> _variables;

        public MathGrammarVisitor(IDictionary<string, decimal> variables)
        {
            _variables = variables;
        }

        public override decimal VisitStart(MathGrammarParser.StartContext context)
        {
            return Visit(context.expression());
        }

        public override decimal VisitExpression(MathGrammarParser.ExpressionContext context)
        {
            if (context.NUMBER() != null)
            {
                return decimal.Parse(context.NUMBER().GetText());
            }

            if (context.variable() != null)
            {
                return Visit(context.variable());
            }

            if (context.GetChild(1).GetText() == "+")
            {
                return Visit(context.expression(0)) + Visit(context.expression(1));
            }

            if (context.GetChild(1).GetText() == "-")
            {
                return Visit(context.expression(0)) - Visit(context.expression(1));
            }

            if (context.GetChild(1).GetText() == "*")
            {
                return Visit(context.expression(0)) * Visit(context.expression(1));
            }

            if (context.GetChild(1).GetText() == "/")
            {
                return Visit(context.expression(0)) / Visit(context.expression(1));
            }

            if (context.GetChild(1).GetText() == "of")
            {
                // "of" assumed to mean multiplication
                return Visit(context.expression(0)) * Visit(context.expression(1));
            }

            if (context.GetText().StartsWith("(") && context.GetText().EndsWith(")"))
            {
                return Visit(context.expression(0));
            }

            throw new NotSupportedException($"Unsupported operation: {context.GetText()}");
        }

        public override decimal VisitVariable(MathGrammarParser.VariableContext context)
        {
            var variableName = context.GetText();
            if (_variables.TryGetValue(variableName, out var value))
            {
                return value;
            }

            throw new KeyNotFoundException($"Variable not found: {variableName}");
        }
    }

}
