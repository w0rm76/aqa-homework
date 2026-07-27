using System;

namespace Battleship
{
    // Игрок-Компьютер
    public class ComputerPlayer : IPlayer, IShooter
    {
        public string Name { get; }
        public Board MyBoard { get; }
        private Random random = new Random();

        public ComputerPlayer(string name, int boardSize)
        {
            Name = name;
            MyBoard = new Board(name, boardSize);
        }

        public Shot Shoot(Board targetBoard)
        {
            int x = random.Next(0, targetBoard.Size);
            int y = random.Next(0, targetBoard.Size);
            Position targetPos = new Position(x, y);

            Ship hitShip = targetBoard.FindShip(targetPos);
            return new Shot(targetBoard, targetPos, hitShip);
        }
    }
}