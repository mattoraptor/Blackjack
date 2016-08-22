using System.Collections.Generic;
using Blackjack;
using NUnit.Framework;

namespace BlackjackTests
{
    public class TestConsoleWrapper : IConsoleWrapper
    {
        public List<string> Lines = new List<string>();

        public void WriteLine(string line)
        {
            Lines.Add(line);
        }

        public string GetInput()
        {
            return "h";
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void AfterWelcomeStartsNewHandAndAsksForWager()
        {
            var consoleWrapper = new TestConsoleWrapper();
            Program.Go(consoleWrapper);
            Assert.That(consoleWrapper.Lines[1],
                Does.StartWith("What would you like to wager ($1 to $50)?"));
            Assert.That(consoleWrapper.Lines[2],
                Does.StartWith("Your cards are "));
        }

        [Test]
        public void PrintsWelcomeMessageOnStartOfGame()
        {
            var consoleWrapper = new TestConsoleWrapper();
            Program.Go(consoleWrapper);
            Assert.That(consoleWrapper.Lines[0],
                Is.EqualTo("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000."));
        }
    }
}