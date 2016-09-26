namespace Blackjack
{
    public class Game
    {
        private readonly IConsoleWrapper _consoleWrapper;
        private readonly PlayerHand _playerHand;

        public Game(IConsoleWrapper consoleWrapper, PlayerHand playerHand)
        {
            _consoleWrapper = consoleWrapper;
            _playerHand = playerHand;
        }

        public void Play()
        {
            _consoleWrapper.WriteLine("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000.");
            while (_playerHand.Money > 0)
            {
                _playerHand.PlayHand();
                if (_playerHand.Money >= 1000)
                {
                    _consoleWrapper.WriteLine("You win!");
                    _consoleWrapper.GetInput();
                    return;
                }
            }

            _consoleWrapper.WriteLine("You lose.");
            _consoleWrapper.GetInput();
        }
    }
}