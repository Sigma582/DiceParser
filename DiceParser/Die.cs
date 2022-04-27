using System;
using System.Linq;

namespace DiceParser
{
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
}
