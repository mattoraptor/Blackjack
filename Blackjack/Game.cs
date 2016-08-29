using System;

namespace Blackjack
{
    public class Game
    {
        private readonly ICardGenerator _cardGenerator;
        private readonly IConsoleWrapper _consoleWrapper;
        private int _money;
        private readonly double _payoutRatio = 1.5;

        public Game(IConsoleWrapper consoleWrapper, ICardGenerator cardGenerator)
        {
            _cardGenerator = cardGenerator;
            _consoleWrapper = consoleWrapper;
        }

        public void Play()
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
            var wager = GetWager();
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
                newCard = _cardGenerator.NextCard();
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
                newCard = _cardGenerator.NextCard();
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
                    $"You had {yourCards} and dealer had {dealersCards}. {loseMessage} You now have ${_money} (-${wager})");
            }
            else if (yourCards == dealersCards)
            {
                _consoleWrapper.WriteLine(
                    $"You had {yourCards} and dealer had {dealersCards}. It's a push! You now have ${_money} (+$0))");
            }
            else
            {
                wager = (int)Math.Floor(wager*_payoutRatio);
                _money += wager;
                _consoleWrapper.WriteLine(
                    $"You had {yourCards} and dealer had {dealersCards}. You won! You now have ${_money} (+${wager}).");
            }
        }

        public int GetWager()
        {
            _consoleWrapper.WriteLine("What would you like to wager ($1 to $50)?");
            var wager = _consoleWrapper.GetNumber();
            if (wager > 50)
            {
                _consoleWrapper.WriteLine("You entered above the maximum wager. Wager set to $50.");
                wager = 50;
            } else if (wager < 1)
            {
                _consoleWrapper.WriteLine("You entered below the minimum wager. Wager set to $1.");
                wager = 1;
            }
            return wager;
        }

        private static string GetCardName(int card)
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

        private static int GetCardValue(int card)
        {
            if (card > 10)
                return 10;
            return card;
        }

        private Tuple<int, int> GetNewHand()
        {
            var num1 = _cardGenerator.NextCard();
            var num2 = _cardGenerator.NextCard();
            return new Tuple<int, int>(num1, num2);
        }
    }
}