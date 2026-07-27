using System;

namespace Battleship
{
    // Игрок-Человек
    public class HumanPlayer : IPlayer, IShooter
    {
        public string Name { get; }
        public Board MyBoard { get; }

        public HumanPlayer(string name, int boardSize)
        {
            Name = name;
            MyBoard = new Board(name, boardSize);
        }

        public Shot Shoot(Board targetBoard)
        {
            Console.Write($"\n[{Name}] Введите X и Y через пробел для выстрела по доске '{targetBoard.OwnerName}': ");
            string input = Console.ReadLine();
            string[] parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts == null || parts.Length != 2)
                throw new FormatException("Неверный формат! Нужно ввести ровно два числа через пробел.");

            // ИСПРАВЛЕНО: Добавлены индексы [0] и [1] для парсинга строк
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);

            Position targetPos = new Position(x, y);

            if (targetPos.X >= targetBoard.Size || targetPos.Y >= targetBoard.Size)
                throw new ArgumentOutOfRangeException(nameof(targetPos), "Выстрел выходит за рамки игрового поля!");

            Ship hitShip = targetBoard.FindShip(targetPos);
            return new Shot(targetBoard, targetPos, hitShip);
        }
    }
}