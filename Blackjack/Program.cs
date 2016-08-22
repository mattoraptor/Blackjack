namespace Blackjack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var consoleWrapper = new ConsoleWrapper();
            new Game(consoleWrapper).Go();
        }
    }
}