using DiceParser.Grammar;
using DiceParser.DiceDescriptors;

namespace DiceParser
{
    public interface IDescriptorFactory
    {
        Entry CreateEntry(DiceGrammarParser.EntryContext context);
    }
}