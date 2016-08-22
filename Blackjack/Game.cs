using System;

namespace Blackjack
{
    public class Game
    {
        private readonly IConsoleWrapper _consoleWrapper;
        private int _money;

        public Game(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public void Go()
        {
            _money = 500;
            _consoleWrapper.WriteLine("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000.");
            while (_money > 0)
            {
                PlayHand();
                if (_money >= 1000)
                {
                    _consoleWrapper.WriteLine("You win!");
                    _consoleWrapper.GetInput();
                    return;
                }
            }


            _consoleWrapper.WriteLine("You lose.");
            _consoleWrapper.GetInput();
        }

        public void PlayHand()
        {
            _consoleWrapper.WriteLine("What would you like to wager ($1 to $50)?");
            var wager = 25;
            var yourHand = GetNewHand();

            _consoleWrapper.WriteLine(
                $"Your cards are {GetCardName(yourHand.Item1)} and {GetCardName(yourHand.Item2)}");

            var dealerHand = GetNewHand();
            _consoleWrapper.WriteLine(
                $"The dealer is showing a {GetCardName(dealerHand.Item1)}. Do you (h)it or (s)tay?");

            var input = _consoleWrapper.GetInput();
            _consoleWrapper.WriteLine("");
            while (input != "h" && input != "s")
            {
                _consoleWrapper.WriteLine("Do you (h)it or (s)tay?");
                input = _consoleWrapper.GetInput();
                _consoleWrapper.WriteLine("");
            }

            if (input == "s")
            {
                _consoleWrapper.WriteLine(Environment.NewLine +
                                          $"The dealer flips their other card over. It's a {GetCardName(dealerHand.Item2)}.");
            }
            var newCard = 0;
            if (input == "h")
            {
                var random = new Random();
                newCard = random.Next(1, 14);
                var n = "";
                if (newCard == 1)
                {
                    n = "n";
                }
                _consoleWrapper.WriteLine(
                    $"The dealer slides another card to you. It's a{n} {GetCardName(newCard)}.");
            }

            var yourCards = GetCardValue(yourHand.Item1) + GetCardValue(yourHand.Item2) +
                            GetCardValue(newCard);
            var dealersCards = GetCardValue(dealerHand.Item1) + GetCardValue(dealerHand.Item2);

            if (dealersCards < 17)
            {
                var random = new Random();
                newCard = random.Next(1, 14);
                var n = "";
                if (newCard == 1)
                {
                    n = "n";
                }
                _consoleWrapper.WriteLine(
                    $"The dealer adds another card to their hand. It's a{n} {GetCardName(newCard)}.");
                dealersCards += newCard;
            }

            if (yourCards < dealersCards || yourCards > 21)
            {
                _money -= wager;
                var loseMessage = yourCards > 21 ? "You busted!" : "You lost!";
                _consoleWrapper.WriteLine(
                    $"You had {yourCards} and dealer had {dealersCards}. {loseMessage} You now have ${_money} (-$25)");
            }
            else if (yourCards == dealersCards)
            {
                _consoleWrapper.WriteLine(
                    $"You had {yourCards} and dealer had {dealersCards}. It's a push! You now have ${_money} (+$0))");
            }
            else
            {
                _money += wager;
                _consoleWrapper.WriteLine(
                    $"You had {yourCards} and dealer had {dealersCards}. You won! You now have ${_money} (+$25).");
            }
        }

        public static string GetCardName(int card)
        {
            if (card == 1)
                return "Ace";
            if (card == 11)
                return "Jack";
            if (card == 12)
                return "Queen";
            if (card == 13)
                return "King";
            return card.ToString();
        }

        public static int GetCardValue(int card)
        {
            if (card > 10)
                return 10;
            return card;
        }

        public static Tuple<int, int> GetNewHand()
        {
            var random = new Random();

            var num1 = random.Next(1, 14);
            var num2 = random.Next(1, 14);
            return new Tuple<int, int>(num1, num2);
        }
    }
}