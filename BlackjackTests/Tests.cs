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
            _game.Money = 500;
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
        public void BidMaxWager_IfWageringAboveLimit()
        {
            _consoleWrapper.Number = 999;
            var result = _game.GetWager();

            Assert.That(result, Is.EqualTo(50));
            CollectionAssert.Contains(_consoleWrapper.Lines, "You entered above the maximum wager. Wager set to $50.");
        }

        [Test]
        public void BidMinWager_IfWagerBelowLimit()
        {
            _consoleWrapper.Number = -999;
            var result = _game.GetWager();

            Assert.That(result, Is.EqualTo(1));
            CollectionAssert.Contains(_consoleWrapper.Lines, "You entered below the minimum wager. Wager set to $1.");
        }

        [Test]
        public void LoseByHavingLowerHand()
        {
            _cardGenerator.AddCards(5, 10, 10, 10, 1);
            _consoleWrapper.Number = 10;
            _game.PlayHand();
            CollectionAssert.Contains(_consoleWrapper.Lines,
                "You had 16 and dealer had 20. You lost! You now have $490 (-$10)");
        }

        [Test]
        public void PlayHandUsesEnteredWagerAndOutputsCorrectMessage_LosingCondition()
        {
            _consoleWrapper.Number = 23;
            _game.PlayHand();

            CollectionAssert.Contains(_consoleWrapper.Lines,
                "You had 30 and dealer had 20. You busted! You now have $477 (-$23)");
        }

        [Test]
        public void PlayHandUsesEnteredWagerAndOutputsCorrectMessage_WinningCondition()
        {
            _cardGenerator.AddCards(10, 10, 10, 7, 1, 1);
            _consoleWrapper.Number = 10;
            _game.PlayHand();

            var expected = 15;
            CollectionAssert.Contains(_consoleWrapper.Lines,
                $"You had 21 and dealer had 17. You won! You now have ${expected + 500} (+${expected}).");
        }

        [Test]
        public void PrintsWelcomeMessageOnStartOfGame()
        {
            _game.Play();
            Assert.That(_consoleWrapper.Lines[0],
                Is.EqualTo("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000."));
        }

        [Test]
        public void WageringMoreMoneyThanYouHaveWagersAllOfYourMoney()
        {
            _consoleWrapper.Number = 40;
            var maxWager = 25;
            _game.Money = maxWager;
            var result = _game.GetWager();

            Assert.That(result, Is.EqualTo(maxWager));
            CollectionAssert.Contains(_consoleWrapper.Lines,
                $"You entered above the maximum wager. Wager set to ${maxWager}.");
        }

        [Test]
        public void YouWinIfTheDealerBusts()
        {
            _cardGenerator.AddCards(10, 10, 10, 6, 1, 10);
            _consoleWrapper.Number = 10;
            _game.PlayHand();

            var expected = 15;
            CollectionAssert.Contains(_consoleWrapper.Lines,
                $"You had 21 and dealer had 26. The dealer busted! You now have ${expected + 500} (+${expected}).");
        }
    }
}