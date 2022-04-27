using System;
using Antlr4.Runtime;
using DiceParser.Grammar;

namespace DiceParser
{
    class Program
    {
        static void Main(string[] args)
        {
            MyParseMethod();
            Console.ReadKey();
        }

        public static void MyParseMethod()
        {
            while (true)
            {
                Console.WriteLine("\r\nEnter dice to roll:");
                var str = Console.ReadLine();
                AntlrInputStream inputStream = new AntlrInputStream(str);
                DiceGrammarLexer DiceGrammarLexer = new DiceGrammarLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(DiceGrammarLexer);

                DiceGrammarParser DiceGrammarParser = new DiceGrammarParser(commonTokenStream);
                DiceGrammarParser.EntryContext context = DiceGrammarParser.entry();
                var visitor = new DiceGrammarVisitor();
                visitor.Visit(context);
            }
        }
    }
}
