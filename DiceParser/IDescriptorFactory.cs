using DiceParser.Grammar;

namespace DiceParser
{
    public interface IDescriptorFactory
    {
        Entry CreateEntry(DiceGrammarParser.EntryContext context);
    }
}