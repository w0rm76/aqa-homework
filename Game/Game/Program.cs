namespace RockPaperScissors
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello this is Rock Paper Scissors...and Well");
            Console.WriteLine("How many steps do you want to play? ( 1 to 10 )");
            string userInput = Console.ReadLine();

            if (!int.TryParse(userInput, out int rounds) || !(rounds > 0 && rounds <= 10))
            {
                Console.WriteLine("Exit the game");
                return;
            }
            Console.WriteLine($"You chose {rounds} round(s)");
            Console.Clear();
            
            Player player = new Player("You");
            Player computer = new Player("pc");
            
            Game game = new Game(player, computer, rounds);
            game.Play();

            Console.ReadLine();
        }
    }
}