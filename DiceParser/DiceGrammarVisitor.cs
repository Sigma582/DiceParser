using System;
using Antlr4.Runtime.Tree;
using DiceParser.Grammar;

namespace DiceParser
{
    public class DiceGrammarVisitor : DiceGrammarBaseVisitor<Descriptor>
    {
        private int _indent = 0;

        private IDescriptorFactory _descriptorFactory = new DescriptorFactory();

        private void Write(string text)
        {
            for (int i = 0; i < _indent; i++)
            {
                Console.Write("  ");
            }
            Console.Write($"{text}\r\n");
        }

        public override Descriptor VisitEntry(DiceGrammarParser.EntryContext context) { 
            Console.WriteLine("Parse tree:");
            Write($" Entry {context.GetText()}");
            var e = _descriptorFactory.CreateEntry(context);
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            
            Console.WriteLine($"Rolling: {e.UnresolvedText}");
            Console.WriteLine($"Result : {e.ResolvedText}");

            return res;
        }

        public override Descriptor VisitDefinition(DiceGrammarParser.DefinitionContext context) {
            Write($" Definition {context.GetText()}");
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            return res;
        }

		public override Descriptor VisitRoll(DiceGrammarParser.RollContext context) {
            Write($" Roll {context.GetText()}");
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            return res;
        }

		public override Descriptor VisitComparison(DiceGrammarParser.ComparisonContext context) { 
            Write($" Comparison {context.GetText()}");
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            return res;
        }

		public override Descriptor VisitOperation(DiceGrammarParser.OperationContext context) {
            Write($" Operation {context.GetText()}");
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            return res;
        }

		public override Descriptor VisitDie(DiceGrammarParser.DieContext context) {
            Write($" Die {context.GetText()}");
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            return res;
        }

		public override Descriptor VisitConstant(DiceGrammarParser.ConstantContext context) {
            Write($" Constant {context.GetText()}");
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            return res;
        }

		public override Descriptor VisitLabel(DiceGrammarParser.LabelContext context) {
            Write($" Label {context.GetText()}");
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            return res;
        }

        public override Descriptor VisitTerminal(ITerminalNode node)
        {
            //Write(" Terminal {0}", node.GetText());
            return base.VisitTerminal(node);
        }
    }
}
