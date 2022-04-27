namespace DiceParser
{
    public class Entry : Descriptor
    {
        public Definition Definition { get; set; }
        public object Label { get; set; }

        public override string UnresolvedText => string.IsNullOrEmpty(Label?.ToString()) ? Definition.UnresolvedText : $"{Label}: {Definition.UnresolvedText}";
        public override string ResolvedText =>   string.IsNullOrEmpty(Label?.ToString()) ? Definition.ResolvedText   : $"{Label}: {Definition.ResolvedText}"  ;
    }
}
