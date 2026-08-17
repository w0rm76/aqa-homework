namespace RockPaperScissors
{
    public class Player
    {
        public string Name { get; private set; }
        public int Won { get; private set; } 

        public Player(string name)
        {
            Name = name;
            Won = 0;
        }

        public void IncrementScore()
        {
            Won++;
        }
    }
}