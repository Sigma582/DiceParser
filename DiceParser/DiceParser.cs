using Antlr4.Runtime;
using DiceParser.DiceDescriptors;
using DiceParser.Grammar;

namespace DiceParser
{
    public class DiceParser
    {
        public string Output { get; set; }

        public Descriptor Parse(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            DiceGrammarLexer DiceGrammarLexer = new DiceGrammarLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(DiceGrammarLexer);

            DiceGrammarParser DiceGrammarParser = new DiceGrammarParser(commonTokenStream);
            DiceGrammarParser.EntryContext context = DiceGrammarParser.entry();
            var visitor = new DiceGrammarVisitor();
            var descriptorTree = visitor.Visit(context);
            Output = visitor.Output;
            return descriptorTree;
        }
    }
}
