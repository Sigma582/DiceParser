using System;

namespace DiceParser
{
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
}
