using Blackjack;
using NUnit.Framework;

namespace BlackjackTests
{
    [TestFixture]
    public class GameTests
    {
        [SetUp]
        public void SetUp()
        {
            _consoleWrapper = new TestConsoleWrapper();
            _cardGenerator = new TestCardGenerator();
            _playerHand = new PlayerHand(_consoleWrapper, _cardGenerator);
            _game = new Game(_consoleWrapper, _playerHand);
        }

        private Game _game;
        private TestConsoleWrapper _consoleWrapper;
        private TestCardGenerator _cardGenerator;
        private PlayerHand _playerHand;

        [Test]
        public void PrintsWelcomeMessageOnStartOfGame()
        {
            _consoleWrapper.Inputs.Enqueue("\n");
            _playerHand = new PlayerHand(_consoleWrapper, _cardGenerator, 0);
            _game = new Game(_consoleWrapper, _playerHand);
            _game.Play();
            Assert.That(_consoleWrapper.Lines[0],
                Is.EqualTo("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000."));
        }
    }
}