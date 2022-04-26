using DiceParser.Grammar;

namespace DiceParser
{
    public interface IDescriptorFactory
    {
        Roll CreateRoll(DiceGrammarParser.RollContext context);
    }
}
