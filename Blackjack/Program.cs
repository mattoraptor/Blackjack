using System;

namespace Blackjack
{
    public interface IConsoleWrapper
    {
        void WriteLine(string line);
        string GetInput();
    }

    public class ConsoleWrapper : IConsoleWrapper
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public string GetInput()
        {
            return Console.ReadKey().KeyChar.ToString();
        }
    }

    public class Game
    {
        private readonly IConsoleWrapper _consoleWrapper;

        public Game(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public void Go()
        {
            var money = 500;
            _consoleWrapper.WriteLine("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000.");
            while (money > 0)
            {
                _consoleWrapper.WriteLine("What would you like to wager ($1 to $50)?");
                var wager = 25;
                var yourHand = Program.GetNewHand();

                _consoleWrapper.WriteLine(
                    $"Your cards are {Program.GetCardName(yourHand.Item1)} and {Program.GetCardName(yourHand.Item2)}");

                var dealerHand = Program.GetNewHand();
                _consoleWrapper.WriteLine(
                    $"The dealer is showing a {Program.GetCardName(dealerHand.Item1)}. Do you (h)it or (s)tay?");

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
                                              $"The dealer flips their other card over. It's a {Program.GetCardName(dealerHand.Item2)}.");
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
                        $"The dealer slides another card to you. It's a{n} {Program.GetCardName(newCard)}.");
                }

                var yourCards = Program.GetCardValue(yourHand.Item1) + Program.GetCardValue(yourHand.Item2) +
                                Program.GetCardValue(newCard);
                var dealersCards = Program.GetCardValue(dealerHand.Item1) + Program.GetCardValue(dealerHand.Item2);

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
                        $"The dealer adds another card to their hand. It's a{n} {Program.GetCardName(newCard)}.");
                    dealersCards += newCard;
                }

                if (yourCards < dealersCards || yourCards > 21)
                {
                    money -= wager;
                    var loseMessage = yourCards > 21 ? "You busted!" : "You lost!";
                    _consoleWrapper.WriteLine(
                        $"You had {yourCards} and dealer had {dealersCards}. {loseMessage} You now have ${money} (-$25)");
                }
                else if (yourCards == dealersCards)
                {
                    _consoleWrapper.WriteLine(
                        $"You had {yourCards} and dealer had {dealersCards}. It's a push! You now have ${money} (+$0))");
                }
                else
                {
                    money += wager;
                    _consoleWrapper.WriteLine(
                        $"You had {yourCards} and dealer had {dealersCards}. You won! You now have ${money} (+$25).");
                }
                if (money >= 1000)
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

    public class Program
    {
        public static void Main(string[] args)
        {
            var consoleWrapper = new ConsoleWrapper();
            new Game(consoleWrapper).Go();
        }

        public static int GetCardValue(int card)
        {
            if (card > 10)
                return 10;
            return card;
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

        public static Tuple<int, int> GetNewHand()
        {
            var random = new Random();

            var num1 = random.Next(1, 14);
            var num2 = random.Next(1, 14);
            return new Tuple<int, int>(num1, num2);
        }
    }
}