namespace RockPaperScissors
{
    public class Move
    {
        public int Value { get; private set; }
        
        public string Name => Value switch
        {
            1 => "Rock",
            2 => "Paper",
            3 => "Scissors",
            4 => "Well",
            _ => "Wrong input"
        };

        public Move(int value)
        {
            Value = value;
        }
        
        public bool IsValid(int min = 1, int max = 4)
        {
            return Value >= min && Value <= max;
        }
    }
}