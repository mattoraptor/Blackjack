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
            _cardGenerator = new TestCardGenerator();
            _game = new Game(_consoleWrapper, _cardGenerator);
        }

        private Game _game;
        private TestConsoleWrapper _consoleWrapper;
        private TestCardGenerator _cardGenerator;

        [Test]
        public void AddsCardFromCardGeneratorToYourHandAndDealersHand()
        {
            _cardGenerator.Card = 3;
            _game.PlayHand();
            CollectionAssert.Contains(_consoleWrapper.Lines, "The dealer slides another card to you. It's a 3.");
            CollectionAssert.Contains(_consoleWrapper.Lines, "The dealer adds another card to their hand. It's a 3.");
        }

        [Test]
        public void AfterWelcomeStartsNewHandAndAsksForWager()
        {
            _game.Play();
            Assert.That(_consoleWrapper.Lines[1],
                Does.StartWith("What would you like to wager ($1 to $50)?"));
            Assert.That(_consoleWrapper.Lines[2],
                Does.StartWith("Your cards are "));
        }

        [Test]
        public void AsksForWagerOnEveryHand()
        {
            _game.Play();
            var count = _consoleWrapper.Lines.Count(line => line.Equals("What would you like to wager ($1 to $50)?"));
            Assert.That(count, Is.GreaterThan(1));
        }

        [Test]
        public void PlayHandUsesEnteredWagerAndOutputsCorrectMessage_LosingCondition()
        {
            _consoleWrapper.Number = 23;
            _game.PlayHand();

            CollectionAssert.Contains(_consoleWrapper.Lines, "You had 30 and dealer had 20. You busted! You now have $-23 (-$23)");
        }

        [Test]
        public void PlayHandUsesEnteredWagerAndOutputsCorrectMessage_WinningCondition()
        {
            _cardGenerator.AddCards(10,10,10,7,1,1);
            _consoleWrapper.Number = 23;
            _game.PlayHand();

            CollectionAssert.Contains(_consoleWrapper.Lines, "You had 21 and dealer had 17. You won! You now have $23 (+$23).");
        }

        [Test]
        public void PrintsWelcomeMessageOnStartOfGame()
        {
            _game.Play();
            Assert.That(_consoleWrapper.Lines[0],
                Is.EqualTo("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000."));
        }
    }
}