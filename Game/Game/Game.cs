using System;

namespace RockPaperScissors
{
    public class Game
    {
        private Player _player;
        private Player _computer;
        private int _rounds;
        private int _roundsCounter;
        private Random _random;

        public Game(Player player, Player computer, int rounds)
        {
            _player = player;
            _computer = computer;
            _rounds = rounds;
            _roundsCounter = 0;
            _random = new Random();
        }
        
        public void Play()
        {
            while (_roundsCounter < _rounds)
            {
                Console.WriteLine("Please, choose your next step:");
                Console.WriteLine("\n1 - Rock\n2 - Paper\n3 - Scissors\n4 - Well\n");

                Move playerMove = ReadFromConsole();
                Console.Clear();
                
                if (!playerMove.IsValid())
                {
                    Console.WriteLine("Invalid input");
                    continue; 
                }
                
                Move computerMove = GenerateRandom();
                
                GameResult result = ProcessRound(playerMove, computerMove);

                _roundsCounter++;
                
                result.Print(_roundsCounter);
                Console.WriteLine();
                Console.WriteLine($"Current score is you {_player.Won} to pc {_computer.Won}");
                
                if ((_computer.Won - _player.Won > _rounds - _roundsCounter) || (_player.Won - _computer.Won > _rounds - _roundsCounter))
                {
                    Console.WriteLine($"This is the end");
                    break;
                }
            }
            
            PrintFinalWinner();
        }
        
        private GameResult ProcessRound(Move playerMove, Move computerMove)
        {
            string resultText;

            if (computerMove.Value == playerMove.Value)
            {
                resultText = "That's the Draw, try again";
            }
            else if ((playerMove.Value == 1 && computerMove.Value == 3) ||
                     (playerMove.Value == 2 && (computerMove.Value == 1 || computerMove.Value == 4)) || 
                     (playerMove.Value == 3 && computerMove.Value == 2) || 
                     (playerMove.Value == 4 && (computerMove.Value == 1 || computerMove.Value == 3)))
            {
                _player.IncrementScore(); // userWon++
                resultText = "Congrats! You won";
            }
            else
            {
                _computer.IncrementScore(); // pcWon++
                resultText = "Sorry, you lose";
            }

            return new GameResult(playerMove, computerMove, resultText);
        }
        
        private Move ReadFromConsole()
        {
            string userInput = Console.ReadLine();
            int.TryParse(userInput, out int userChoice);
            return new Move(userChoice);
        }
        
        private Move GenerateRandom()
        {
            int computerChoice = _random.Next(1, 5);
            return new Move(computerChoice);
        }
        
        private void PrintFinalWinner()
        {
            Console.WriteLine("\n=== FINAL RESULTS ===");
            Console.WriteLine($"Total Score -> You: {_player.Won} | PC: {_computer.Won}");
            
            if (_player.Won > _computer.Won)
                Console.WriteLine($"Winner: {_player.Name}");
            else if (_computer.Won > _player.Won)
                Console.WriteLine($"Winner: {_computer.Name}");
            else
                Console.WriteLine("It's a Draw!");
        }
    }
}
