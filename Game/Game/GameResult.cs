using System;

namespace RockPaperScissors
{
    public class GameResult
    {
        private Move _playerMove;
        private Move _computerMove;
        private string _resultText;
        
        public GameResult(Move playerMove, Move computerMove, string resultText)
        {
            _playerMove = playerMove;
            _computerMove = computerMove;
            _resultText = resultText;
        }
        
        public void Print()
        {
            Console.WriteLine($"You chose {_playerMove.Name}");
            Console.WriteLine($"Computer chose {_computerMove.Name}");
            Console.WriteLine(_resultText);
        }
        
        public void Print(int roundNumber)
        {
            Console.WriteLine($"It were played {roundNumber} round(s)");
            Print(); 
        }
    }
}