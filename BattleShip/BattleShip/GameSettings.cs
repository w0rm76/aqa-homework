using System;

namespace Battleship
{
    // ПУНКТ 3: Настройки игры
    public class GameSettings
    {
        public int BoardSize { get; }

        public GameSettings(int boardSize)
        {
            if (boardSize < 4) throw new ArgumentException("Размер поля слишком мал для полноценной игры.");
            BoardSize = boardSize;
        }
    }

    // ПУНКТ 3: Методы расширения (Extension Methods) для класса Random
    public static class RandomExtensions
    {
        public static Ship NextShip(this Random random, GameSettings settings, string name, int length)
        {
            int type = random.Next(0, 2); // 0 - Горизонтальный, 1 - Вертикальный

            if (type == 0) // Horizontal
            {
                int startX = random.Next(0, settings.BoardSize - length + 1);
                int startY = random.Next(0, settings.BoardSize);
                return new HorizontalShip(name, length, new Position(startX, startY));
            }
            else // Vertical
            {
                int startX = random.Next(0, settings.BoardSize);
                int startY = random.Next(0, settings.BoardSize - length + 1);
                return new VerticalShip(name, length, new Position(startX, startY));
            }
        }
    }
}