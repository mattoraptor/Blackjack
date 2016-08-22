﻿using System;

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

    public class Program
    {
        public static void Main(string[] args)
        {
            var consoleWrapper = new ConsoleWrapper();
            Go(consoleWrapper);
        }

        public static void Go(IConsoleWrapper consoleWrapper)
        {
            var money = 500;
            consoleWrapper.WriteLine("Welcome to blackjack. You have $500. Each hand costs $25. You win at $1000.");
            consoleWrapper.WriteLine("What would you like to wager ($1 to $50)?");
            while (money > 0)
            {
                var wager = 25;
                var yourHand = GetNewHand();

                consoleWrapper.WriteLine(
                    $"Your cards are {GetCardName(yourHand.Item1)} and {GetCardName(yourHand.Item2)}");

                var dealerHand = GetNewHand();
                consoleWrapper.WriteLine(
                    $"The dealer is showing a {GetCardName(dealerHand.Item1)}. Do you (h)it or (s)tay?");

                var input = consoleWrapper.GetInput();
                consoleWrapper.WriteLine("");
                while (input != "h" && input != "s")
                {
                    consoleWrapper.WriteLine("Do you (h)it or (s)tay?");
                    input = consoleWrapper.GetInput();
                    consoleWrapper.WriteLine("");
                }

                if (input == "s")
                {
                    consoleWrapper.WriteLine(Environment.NewLine +
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
                    consoleWrapper.WriteLine($"The dealer slides another card to you. It's a{n} {GetCardName(newCard)}.");
                }

                var yourCards = GetCardValue(yourHand.Item1) + GetCardValue(yourHand.Item2) + GetCardValue(newCard);
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
                    consoleWrapper.WriteLine(
                        $"The dealer adds another card to their hand. It's a{n} {GetCardName(newCard)}.");
                    dealersCards += newCard;
                }

                if (yourCards < dealersCards || yourCards > 21)
                {
                    money -= wager;
                    var loseMessage = yourCards > 21 ? "You busted!" : "You lost!";
                    consoleWrapper.WriteLine(
                        $"You had {yourCards} and dealer had {dealersCards}. {loseMessage} You now have ${money} (-$25)");
                }
                else if (yourCards == dealersCards)
                {
                    consoleWrapper.WriteLine(
                        $"You had {yourCards} and dealer had {dealersCards}. It's a push! You now have ${money} (+$0))");
                }
                else
                {
                    money += wager;
                    consoleWrapper.WriteLine(
                        $"You had {yourCards} and dealer had {dealersCards}. You won! You now have ${money} (+$25).");
                }
                if (money >= 1000)
                {
                    consoleWrapper.WriteLine("You win!");
                    consoleWrapper.GetInput();
                    return;
                }
            }


            consoleWrapper.WriteLine("You lose.");
            consoleWrapper.GetInput();
        }

        private static int GetCardValue(int card)
        {
            if (card > 10)
                return 10;
            return card;
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

        private static Tuple<int, int> GetNewHand()
        {
            var random = new Random();

            var num1 = random.Next(1, 14);
            var num2 = random.Next(1, 14);
            return new Tuple<int, int>(num1, num2);
        }
    }
}