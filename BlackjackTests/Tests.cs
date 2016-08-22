using System.Linq;
using Blackjack;
using NUnit.Framework;

namespace BlackjackTests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void SetUp()
        {
            _consoleWrapper = new TestConsoleWrapper();
            _game = new Game(_consoleWrapper);
        }

        private Game _game;
        private TestConsoleWrapper _consoleWrapper;

        [Test]
        public void AfterWelcomeStartsNewHandAndAsksForWager()
        {
            _game.Go();
            Assert.That(_consoleWrapper.Lines[1],
                Does.StartWith("What would you like to wager ($1 to $50)?"));
            Assert.That(_consoleWrapper.Lines[2],
                Does.StartWith("Your cards are "));
        }

        [Test]
        public void AsksForWagerOnEveryHand()
        {
            _game.Go();
            var count = _consoleWrapper.Lines.Count(line => line.Equals("What would you like to wager ($1 to $50)?"));
            Assert.That(count, Is.GreaterThan(1));
        }

        [Test]
        public void PrintsWelcomeMessageOnStartOfGame()
        {
            _game.Go();
            Assert.That(_consoleWrapper.Lines[0],
                Is.EqualTo("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000."));
        }
    }
}