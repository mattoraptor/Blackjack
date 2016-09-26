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
            _playerHand = new PlayerHand(_consoleWrapper, _cardGenerator);
            _game = new Game(_consoleWrapper, _playerHand);
            _playerHand.Money = 500;
        }

        private Game _game;
        private TestConsoleWrapper _consoleWrapper;
        private TestCardGenerator _cardGenerator;
        private PlayerHand _playerHand;

        private void EnqueueHit()
        {
            _consoleWrapper.Inputs.Enqueue("h");
        }

        [Test]
        public void AddsCardFromCardGeneratorToYourHandAndDealersHand()
        {
            EnqueueHit();
            _cardGenerator.Card = 3;
            _playerHand.PlayHand();
            CollectionAssert.Contains(_consoleWrapper.Lines, "The dealer slides another card to you. It's a 3.");
            CollectionAssert.Contains(_consoleWrapper.Lines, "The dealer adds another card to their hand. It's a 3.");
        }

        [Test]
        public void AfterWelcomeStartsNewHandAndAsksForWager()
        {
            _consoleWrapper.Inputs.Enqueue("h");
            _playerHand.PlayHand();
            Assert.That(_consoleWrapper.Lines[0],
                Does.StartWith("What would you like to wager ($1 to $50)?"));
            Assert.That(_consoleWrapper.Lines[1],
                Does.StartWith("Your cards are "));
        }

        [Test]
        public void AsksAgainOnInvalidInput()
        {
            _consoleWrapper.Inputs.Enqueue("qwerty");
            _consoleWrapper.Inputs.Enqueue("h");
            _playerHand.PlayerHitsOrStays(new Hand(1, 2));

            var questionCount = 0;
            foreach (var line in _consoleWrapper.Lines)
                if (line.EndsWith("Do you (h)it or (s)tay?"))
                    questionCount += 1;

            Assert.That(questionCount, Is.EqualTo(2));
        }

        [Test]
        public void AsksForWagerOnHand()
        {
            _consoleWrapper.Inputs.Enqueue("h");
            _playerHand.PlayHand();
            var count = _consoleWrapper.Lines.Count(line => line.Equals("What would you like to wager ($1 to $50)?"));
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void BidMaxWager_IfWageringAboveLimit()
        {
            _consoleWrapper.Number = 999;
            var result = _playerHand.GetWager();

            Assert.That(result, Is.EqualTo(50));
            CollectionAssert.Contains(_consoleWrapper.Lines, "You entered above the maximum wager. Wager set to $50.");
        }

        [Test]
        public void BidMinWager_IfWagerBelowLimit()
        {
            _consoleWrapper.Number = -999;
            var result = _playerHand.GetWager();

            Assert.That(result, Is.EqualTo(1));
            CollectionAssert.Contains(_consoleWrapper.Lines, "You entered below the minimum wager. Wager set to $1.");
        }

        [Test]
        public void LoseByHavingLowerHand()
        {
            EnqueueHit();
            _cardGenerator.AddCards(5, 10, 10, 10, 1);
            _consoleWrapper.Number = 10;
            _playerHand.PlayHand();
            CollectionAssert.Contains(_consoleWrapper.Lines,
                "You had 16 and dealer had 20. You lost! You now have $490 (-$10)");
        }

        [Test]
        public void PlayerAcesAreWorthElevenIfNotABust()
        {
            EnqueueHit();
            // Player, Player, Dealer, Dealer, Player, Dealer
            _cardGenerator.AddCards(5, 5, 10, 6, 1, 2);
            _consoleWrapper.Number = 10;
            _playerHand.PlayHand();

            var expected = 15;
            CollectionAssert.Contains(_consoleWrapper.Lines,
                $"You had 21 and dealer had 18. You won! You now have ${expected + 500} (+${expected}).");
        }

        [Test]
        public void PlayHandUsesEnteredWagerAndOutputsCorrectMessage_LosingCondition()
        {
            EnqueueHit();
            _consoleWrapper.Number = 23;
            _playerHand.PlayHand();

            CollectionAssert.Contains(_consoleWrapper.Lines,
                "You had 30 and dealer had 20. You busted! You now have $477 (-$23)");
        }

        [Test]
        public void PlayHandUsesEnteredWagerAndOutputsCorrectMessage_WinningCondition()
        {
            EnqueueHit();
            _cardGenerator.AddCards(10, 10, 10, 7, 1, 1);
            _consoleWrapper.Number = 10;
            _playerHand.PlayHand();

            var expected = 15;
            CollectionAssert.Contains(_consoleWrapper.Lines,
                $"You had 21 and dealer had 17. You won! You now have ${expected + 500} (+${expected}).");
        }

        [Test]
        public void PrintsWelcomeMessageOnStartOfGame()
        {
            _consoleWrapper.Inputs.Enqueue("\n");
            _playerHand.Money = 0;
            _game = new Game(_consoleWrapper, _playerHand);
            _game.Play();
            Assert.That(_consoleWrapper.Lines[0],
                Is.EqualTo("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000."));
        }

        [Test]
        public void PrintsYourHand()
        {
            EnqueueHit();
            _cardGenerator.AddCards(2, 3, 4);
            _playerHand.PlayHand();

            CollectionAssert.Contains(_consoleWrapper.Lines,
                $"Your cards are {2} and {3}");
        }

        [Test]
        public void WageringMoreMoneyThanYouHaveWagersAllOfYourMoney()
        {
            _consoleWrapper.Number = 40;
            var maxWager = 25;
            _playerHand.Money = maxWager;
            var result = _playerHand.GetWager();

            Assert.That(result, Is.EqualTo(maxWager));
            CollectionAssert.Contains(_consoleWrapper.Lines,
                $"You entered above the maximum wager. Wager set to ${maxWager}.");
        }

        [Test]
        public void YouWinIfTheDealerBusts()
        {
            EnqueueHit();
            _cardGenerator.AddCards(10, 10, 10, 6, 1, 10);
            _consoleWrapper.Number = 10;
            _playerHand.PlayHand();

            var expected = 15;
            CollectionAssert.Contains(_consoleWrapper.Lines,
                $"You had 21 and dealer had 26. The dealer busted! You now have ${expected + 500} (+${expected}).");
        }
    }
}