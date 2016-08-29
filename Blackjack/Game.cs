using System;

namespace Blackjack
{
    public class Game
    {
        private readonly ICardGenerator _cardGenerator;
        private readonly IConsoleWrapper _consoleWrapper;
        public int Money;
        private readonly double _payoutRatio = 1.5;

        public Game(IConsoleWrapper consoleWrapper, ICardGenerator cardGenerator)
        {
            _cardGenerator = cardGenerator;
            _consoleWrapper = consoleWrapper;
        }

        public void Play()
        {
            Money = 500;
            _consoleWrapper.WriteLine("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000.");
            while (Money > 0)
            {
                PlayHand();
                if (Money >= 1000)
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

            if (yourCards > 21)
            {
                var loseMessage = "You busted!";
                DoYouLost(wager, yourCards, dealersCards, loseMessage);
            }
            else if (dealersCards > 21)
            {
                var winMessage = "The dealer busted!";
                DoYouWon(wager, yourCards, dealersCards, winMessage);
            }
            else if (yourCards < dealersCards )
            {
                var loseMessage = "You lost!";
                DoYouLost(wager, yourCards, dealersCards, loseMessage);
            }
            else if (yourCards == dealersCards)
            {
                var itSAPush = "It's a push!";
                _consoleWrapper.WriteLine(
                    $"You had {yourCards} and dealer had {dealersCards}. {itSAPush} You now have ${Money} (+$0))");
            }
            else
            {
                var winMessage = "You won!";
                DoYouWon(wager, yourCards, dealersCards, winMessage);
            }
        }

        private void DoYouWon(int wager, int yourCards, int dealersCards, string winMessage)
        {
            wager = (int) Math.Floor(wager*_payoutRatio);
            Money += wager;
            _consoleWrapper.WriteLine(
                $"You had {yourCards} and dealer had {dealersCards}. {winMessage} You now have ${Money} (+${wager}).");
        }

        private void DoYouLost(int wager, int yourCards, int dealersCards, string loseMessage)
        {
            Money -= wager;
            _consoleWrapper.WriteLine(
                $"You had {yourCards} and dealer had {dealersCards}. {loseMessage} You now have ${Money} (-${wager})");
        }

        public int GetWager()
        {
            int maxWager = 50;
            _consoleWrapper.WriteLine($"What would you like to wager ($1 to ${maxWager})?");
            var wager = _consoleWrapper.GetNumber();
            if (wager > Money)
            {
                _consoleWrapper.WriteLine($"You entered above the maximum wager. Wager set to ${ Money}.");
                wager = Money;
            }
            if (wager > maxWager)
            {
                _consoleWrapper.WriteLine($"You entered above the maximum wager. Wager set to ${maxWager}.");
                wager = maxWager;
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