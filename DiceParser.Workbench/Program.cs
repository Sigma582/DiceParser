using System;

namespace DiceParser.Workbench
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
                var diceParser = new DiceParser();
                var rollDescription = diceParser.Parse(str);

                Console.WriteLine(diceParser.Output);
                Console.WriteLine($"Rolling: {rollDescription.UnresolvedText}");
                Console.WriteLine($"Result: {rollDescription.ResolvedText}");
            }
        }
    }
}
