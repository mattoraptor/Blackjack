using System.Collections.Generic;
using Blackjack;

namespace BlackjackTests
{
    public class TestConsoleWrapper : IConsoleWrapper
    {
        public Queue<string> Inputs = new Queue<string>();
        public List<string> Lines = new List<string>();
        public int Number = 50;

        public void WriteLine(string line)
        {
            Lines.Add(line);
        }

        public string GetInput()
        {
            return Inputs.Dequeue();
        }

        public int GetNumber()
        {
            return Number;
        }
    }
}