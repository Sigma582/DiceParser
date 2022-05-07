using System;
using System.Linq;
using Antlr4.Runtime.Tree;
using DiceParser.DiceDescriptors;
using DiceParser.Grammar;

namespace DiceParser
{
    public class DescriptorFactory : IDescriptorFactory
    {
        public Entry CreateEntry(DiceGrammarParser.EntryContext context)
        {
            var result = new Entry();

            var definitionContext = context.children[0] as DiceGrammarParser.DefinitionContext;
            if (definitionContext != null)
            {
                result.Definition = CreateDefinition(definitionContext);
            }

            if (context.children.Count > 1)
            {
                var labelContext = context.children[1] as DiceGrammarParser.LabelContext;
                if (labelContext != null)
                {
                    result.Label = labelContext.children[0].GetText();
                }
            }
            return result;
        }

        public Definition CreateDefinition(DiceGrammarParser.DefinitionContext context)
        {
            if (context.children.Count == 1)
            {
                var roll = context.children[0] as DiceGrammarParser.RollContext;
                return CreateRoll(roll);
            }

            return CreateComparison(context);
        }

        public Roll CreateRoll(DiceGrammarParser.RollContext context)
        {
            if (context.children.Count == 1)
            {
                return CreateSingleRoll(context);
            }

            return CreateCompoundRoll(context);
        }

        private Roll CreateSingleRoll(DiceGrammarParser.RollContext context)
        {
            var die = context.children[0] as DiceGrammarParser.DieContext;
            if (die != null)
            {
                return CreateDie(die);
            }

            var constant = context.children[0] as DiceGrammarParser.ConstantContext;
            if (constant != null)
            {
                return CreateConstant(constant);
            }

            throw new ApplicationException();
        }

        private Roll CreateCompoundRoll(DiceGrammarParser.RollContext context)
        {
            if (context.children.Count != 3)
            {
                throw new ApplicationException();
            }

            var leftContext = context.children[0] as DiceGrammarParser.RollContext;
            var operationContext = context.children[1] as DiceGrammarParser.OperationContext;
            var rightContext = context.children[2] as DiceGrammarParser.RollContext;

            if (leftContext is null || rightContext is null || operationContext is null)
            {
                throw new ApplicationException();
            }

            var left = CreateRoll(leftContext);
            var op = operationContext.children.OfType<TerminalNodeImpl>().FirstOrDefault()?.GetText();
            var right = CreateRoll(rightContext);

            return new CompoundRoll(left, op, right);
        }

        private Roll CreateDie(DiceGrammarParser.DieContext context)
        {
            var term = context.children[0] as TerminalNodeImpl;
            var text = term?.GetText();

            var parts = text.Split('d', StringSplitOptions.RemoveEmptyEntries);

            var qty = 1;
            var size = 1;
            if(parts.Length == 1)
            {
                size = int.Parse(parts[0]);
            }
            if(parts.Length == 2)
            {
                qty = int.Parse(parts[0]);
                size = int.Parse(parts[1]);
            }

            return new Die(qty, size);
        }

        private Roll CreateConstant(DiceGrammarParser.ConstantContext context)
        {
            var term = context.children[0] as TerminalNodeImpl;
            var text = term?.GetText();

            return new Constant(int.Parse(text));
        }

        private Comparison CreateComparison(DiceGrammarParser.DefinitionContext context)
        {
            var leftContext = context.children[0] as DiceGrammarParser.RollContext;
            var opContext = context.children[1] as DiceGrammarParser.ComparisonContext;
            var rightContext = context.children[2] as DiceGrammarParser.RollContext;

            var left = CreateRoll(leftContext);
            var op = opContext.children.OfType<TerminalNodeImpl>().FirstOrDefault()?.GetText();
            var right = CreateRoll(rightContext);

            return new Comparison(left, op, right);
        }
    }
}
