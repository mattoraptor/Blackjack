namespace Blackjack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var consoleWrapper = new ConsoleWrapper();
            ICardGenerator cardGenerator = new RandomCardGenerator();
            new Game(consoleWrapper, new PlayerHand(consoleWrapper, cardGenerator)).Play();
        }
    }
}