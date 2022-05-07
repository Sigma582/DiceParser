using System;

namespace DiceParser.DiceDescriptors
{
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
