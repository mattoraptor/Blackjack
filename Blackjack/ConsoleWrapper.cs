using System;

namespace Blackjack
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public string GetInput()
        {
            return Console.ReadKey().KeyChar.ToString();
        }

        public int GetNumber()
        {
            return int.Parse(Console.ReadLine());
        }
    }
}