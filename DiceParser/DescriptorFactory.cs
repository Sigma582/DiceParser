using System;
using System.Linq;
using DiceParser.Grammar;

namespace DiceParser
{
    public class DescriptorFactory : IDescriptorFactory
    {
        public Entry CreateEntry(DiceGrammarParser.EntryContext context)
        {
            var result = new Entry();

            var definitionContext = context.children.OfType<DiceGrammarParser.DefinitionContext>().FirstOrDefault();

            if (definitionContext != null)
            {
                result.Definition = CreateDefinition(definitionContext);
            }

            return result;
        }

        public Definition CreateDefinition(DiceGrammarParser.DefinitionContext context)
        {
            var result = new Definition();
            if (context.children.Count > 0)
            {
                var leftRollContext = context.children[0] as DiceGrammarParser.RollContext;
                result.LeftRoll = CreateRoll(leftRollContext);
            }

            if (context.children.Count > 2)
            {
                var comparisonContext = context.children[1] as DiceGrammarParser.ComparisonContext;
                result.Comparison = new Comparison(comparisonContext);
                var rightRollContext = context.children[2] as DiceGrammarParser.RollContext;
                result.RightRoll = CreateRoll(rightRollContext);
            }
            return result;
        }

        public Roll CreateRoll(DiceGrammarParser.RollContext context)
        {
            if (context.children.Count == 0)
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

        private Roll CreateDie(DiceGrammarParser.DieContext die)
        {
            return new Die();
        }

        private Roll CreateConstant(DiceGrammarParser.ConstantContext constant)
        {
            throw new NotImplementedException();
        }

        private Roll CreateCompoundRoll(DiceGrammarParser.RollContext context)
        {
            if (context.children.Count != 3)
            {
                throw new ApplicationException();
            }

            var left = context.children[0] as DiceGrammarParser.RollContext;
            var operation = context.children[1] as DiceGrammarParser.OperationContext;
            var right = context.children[2] as DiceGrammarParser.RollContext;

            if (left is null || right is null || operation is null)
            {
                throw new ApplicationException();
            }

            return new CompoundRoll(left, operation, right);
        }
    }
}
