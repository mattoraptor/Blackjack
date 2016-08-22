using System.Collections.Generic;
using Blackjack;

namespace BlackjackTests
{
    public class TestConsoleWrapper : IConsoleWrapper
    {
        public List<string> Lines = new List<string>();
        public int Number = 50;

        public void WriteLine(string line)
        {
            Lines.Add(line);
        }

        public string GetInput()
        {
            return "h";
        }

        public int GetNumber()
        {
            return Number;
        }
    }
}