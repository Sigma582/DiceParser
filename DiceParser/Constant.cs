namespace DiceParser
{
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
}
