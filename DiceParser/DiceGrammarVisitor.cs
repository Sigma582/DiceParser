using Antlr4.Runtime.Tree;
using DiceParser.Grammar;
using DiceParser.DiceDescriptors;
using System.Text;

namespace DiceParser
{
    public class DiceGrammarVisitor : DiceGrammarBaseVisitor<Descriptor>
    {
        private StringBuilder _outputStringBuilder = new StringBuilder(); 

        private int _indent = 0;

        private IDescriptorFactory _descriptorFactory = new DescriptorFactory();

        public string Output => _outputStringBuilder.ToString();

        private void Write(string text)
        {
            for (int i = 0; i < _indent; i++)
            {
                _outputStringBuilder.Append("  ");
            }
            _outputStringBuilder.Append($"{text}\r\n");
        }

        public override Descriptor VisitEntry(DiceGrammarParser.EntryContext context) { 
            StringBuilder Output = new StringBuilder(); 

            Output.AppendLine("Parse tree:");
            Write($" Entry {context.GetText()}");
            var e = _descriptorFactory.CreateEntry(context);
            _indent++;
            var res = VisitChildren(context);
            _indent--;
            
            Output.AppendLine($"Rolling: {e.UnresolvedText}");
            Output.AppendLine($"Result : {e.ResolvedText}");

            return e;
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
            //Append(" Terminal {0}", node.GetText());
            return base.VisitTerminal(node);
        }
    }
}
