using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DiceParser.Grammar;
using System.Linq;

namespace DiceParser
{
    class Program
    {
        static void Main(string[] args)
        {
            MyParseMethod();
            Console.ReadKey();
        }

        public static void MyParseMethod()
        {
            while (true)
            {
                //d100+d20+10>d100-d8
                Console.WriteLine("\r\nEnter dice to roll:");
                var str = Console.ReadLine();
                AntlrInputStream inputStream = new AntlrInputStream(str);
                DiceGrammarLexer DiceGrammarLexer = new DiceGrammarLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(DiceGrammarLexer);

                DiceGrammarParser DiceGrammarParser = new DiceGrammarParser(commonTokenStream);
                DiceGrammarParser.EntryContext context = DiceGrammarParser.entry();
                var visitor = new DiceGrammarVisitor();
                visitor.Visit(context);
            }
        }
    }

    public class MyVisitor : DiceGrammarBaseVisitor<object>
    {
        public override object VisitEntry(DiceGrammarParser.EntryContext context)
        {
            Console.WriteLine("DiceGrammarVisitor VisitR");
            context
                .children
                .OfType<TerminalNodeImpl>()
                .ToList()
                .ForEach(child => Visit(child));
            return null;
        }

        private void Visit(TerminalNodeImpl node)
        {
            Console.WriteLine(" Visit Symbol={0}", node.Symbol.Text);
        }
    }

    public class DiceGrammarVisitor: DiceGrammarBaseVisitor<Descriptor>
    {
        private int _indent = 0;

        private IDescriptorFactory _rollFactory = new DescriptorFactory();

        private void Write(string text)
        {
            for (int i = 0; i < _indent; i++)
            {
                Console.Write("  ");
            }
            Console.Write($"{text}\r\n");
        }

        public override Descriptor VisitEntry(DiceGrammarParser.EntryContext context) { 
            Write($" Entry {context.GetText()}");
            //return new Entry(context);
            _indent++;
            var res = VisitChildren(context);
            _indent--;
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

    public abstract class Descriptor
    {
        public string Text { get; protected set; }
    }

    public class Entry : Descriptor
    {
        public Definition Definition { get; set; }
        public object Label { get; set; }
    }

    public class Definition : Descriptor
    {

        public Roll LeftRoll { get; set; }
        public Roll RightRoll { get; set; }
        public Comparison Comparison { get; set; }
    }

    public abstract class Roll : Descriptor
    {
        public int Value { get; protected set; }

        //public Roll(DiceGrammarParser.RollContext context)
        //{
        //    if (context.children.Count > 0)
        //    {
        //        var dieContext = context.children[0] as DiceGrammarParser.DieContext;
        //        if (dieContext != null) FirstElement = new Die(dieContext);
        //    }

        //    if (context.children.Count > 2)
        //    {
        //        var operationContext = context.children[1] as DiceGrammarParser.OperationContext;
        //        Operation = new Operation(operationContext);
        //        var nestedRollContext = context.children[2] as DiceGrammarParser.RollContext;
        //        NestedRoll = new Roll(nestedRollContext);
        //    }
        //}

        //public int Value { get; protected set; }

        //RollElement FirstElement { get; set; }
        //Operation Operation { get; set; }
        //Roll NestedRoll { get; set; }
    }

    public class Die : Roll
    {
        public Die(int qty, int dieSize)
        {
            Qty = qty;
            DieSize = dieSize;
            Value = new Random().Next(1, dieSize);
            Text = $"d{dieSize}";
        }

        public int Qty { get; }
        public int DieSize { get; }
    }

    public class Constant : Roll
    {
        public Constant(int value)
        {
            Value = value;
            Text = $"{value}";
        }

        public Constant(DiceGrammarParser.ConstantContext constant)
        {
        }
    }

    public class CompoundRoll : Roll
    {
        public CompoundRoll(DiceGrammarParser.RollContext left, DiceGrammarParser.OperationContext operation, DiceGrammarParser.RollContext right)
        {
            
        }
    }

    public class Comparison : Descriptor
    {
        public Comparison(string comparisonOperator)
        {
            ComparisonOperator = comparisonOperator;
            switch (ComparisonOperator)
            {
                case "<=":
                    ComparisonDelegate = (left, right) => left.Value <= right.Value;
                    break;
                case "<":
                    ComparisonDelegate = (left, right) => left.Value <= right.Value;
                    break;
                case "=":
                    ComparisonDelegate = (left, right) => left.Value <= right.Value;
                    break;
                case ">":
                    ComparisonDelegate = (left, right) => left.Value <= right.Value;
                    break;
                case ">=":
                    ComparisonDelegate = (left, right) => left.Value <= right.Value;
                    break;
                default:
                    throw new ArgumentException("Comparison operator must be one of the following: <=, <, =, >, >=", nameof(comparisonOperator));
            }
        }

        public Comparison(DiceGrammarParser.ComparisonContext comparisonContext)
        {
        }

        private Func<Roll, Roll, bool> ComparisonDelegate;

        public string ComparisonOperator { get; }

        public bool Compare(Roll left, Roll right)
        {
            return ComparisonDelegate(left, right);
        }
    }

    public class Operation : Descriptor
    {
        public Operation(DiceGrammarParser.OperationContext operationContext)
        {
        }
    }
}
