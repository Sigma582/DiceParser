namespace DiceParser.DiceDescriptors
{
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
}
