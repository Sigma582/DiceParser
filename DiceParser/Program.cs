using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DiceParser.Grammar;
using System.Linq;
using System.Collections.Generic;

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

    public abstract class Descriptor
    {
        public string Text { get; protected set; }
        public abstract string UnresolvedText { get; }
        public abstract string ResolvedText { get; }

        public override string ToString()
        {
            return $"{UnresolvedText}\r\n{ResolvedText}"; ;
        }
    }

    public class Entry : Descriptor
    {
        public Definition Definition { get; set; }
        public object Label { get; set; }

        public override string UnresolvedText => string.IsNullOrEmpty(Label?.ToString()) ? Definition.UnresolvedText : $"{Label}: {Definition.UnresolvedText}";
        public override string ResolvedText =>   string.IsNullOrEmpty(Label?.ToString()) ? Definition.ResolvedText   : $"{Label}: {Definition.ResolvedText}"  ;
    }

    public abstract class Definition : Descriptor
    {
    }

    public abstract class Roll : Definition
    {
        public int Value { get; protected set; }
        public List<int> Values { get; protected set; } = new List<int>();
    }

    public class Die : Roll
    {
        public Die(int qty, int dieSize)
        {
            Qty = qty;
            DieSize = dieSize;
            
            var r = new Random();
            for (int i = 0; i < qty; i++)
            {
                var dieResult = r.Next(1, dieSize);
                Value += dieResult;
                Values.Add(dieResult);
            }

            Text = $"d{dieSize}";
        }

        public int Qty { get; }
        public int DieSize { get; }

        public override string UnresolvedText => Qty > 1 ? $"{Qty}d{DieSize}" : $"d{DieSize}";
        public override string ResolvedText => Values.Aggregate("", (s, v) => $"{s}+{v}").Trim('+');
    }

    public class Constant : Roll
    {
        public Constant(int value)
        {
            Value = value;
            Values.Add(value);
            Text = $"{value}";
        }

        public override string UnresolvedText => $"{Value}";
        public override string ResolvedText => $"{Value}";

    }

    public class CompoundRoll : Roll
    {
        public Roll Left { get; set; }
        public Roll Right { get; set; }
        public string Operator { get; }

        public CompoundRoll(Roll left, string op, Roll right)
        {
            Left = left;
            Right = right;
            Operator = op;

            switch (op)
            {
                case "+":
                    Value = Left.Value + Right.Value;
                    break;
                    
                case "-":
                    Value = Left.Value - Right.Value;
                    break;

                default:
                    throw new ArgumentException("Operator must be one of the following: +, -", nameof(op));
            }
            Values.Add(Left.Value);
            Values.Add(Right.Value);
        }

        public override string ToString()
        {
            return $"{Left} {Operator} {Right}";
        }

        public override string UnresolvedText => $"{Left.UnresolvedText} {Operator} {Right.UnresolvedText}";
        public override string ResolvedText => $"{Left.ResolvedText} {Operator} {Right.ResolvedText}";
    }

    public class Comparison : Definition
    {
        public Roll Left { get; set; }
        public Roll Right { get; set; }
        public string Operator { get; }
        public string Result { get; }

        private Func<Roll, Roll, bool> ComparisonDelegate;

        public Comparison(Roll left, string op, Roll right)
        {
            Left = left;
            Right = right;
            Operator = op;

            switch (Operator)
            {
                case "<=":
                    ComparisonDelegate = (left, right) => left.Value <= right.Value;
                    break;
                case "<":
                    ComparisonDelegate = (left, right) => left.Value < right.Value;
                    break;
                case "=":
                    ComparisonDelegate = (left, right) => left.Value == right.Value;
                    break;
                case ">":
                    ComparisonDelegate = (left, right) => left.Value > right.Value;
                    break;
                case ">=":
                    ComparisonDelegate = (left, right) => left.Value >= right.Value;
                    break;
                default:
                    throw new ArgumentException("Comparison operator must be one of the following: <=, <, =, >, >=", nameof(op));
            }

            Result = ComparisonDelegate(left, right) ? "Success" : "Failure";
        }

        public bool Compare(Roll left, Roll right)
        {
            return ComparisonDelegate(left, right);
        }

        public override string UnresolvedText => $"{Left.UnresolvedText} {Operator} {Right.UnresolvedText}";
        public override string ResolvedText => $"{Left.ResolvedText} {Operator} {Right.ResolvedText}; {Left.Value} {Operator} {Right.Value} => {Result}";

    }
}
