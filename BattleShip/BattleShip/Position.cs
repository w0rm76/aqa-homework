using System;

namespace Battleship
{
    // Требование 1.4: readonly struct Position для координат
    public readonly struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            // Валидация координат в конструкторе (Занятие 6 / Пункт 1)
            if (x < 0 || y < 0)
                throw new ArgumentOutOfRangeException("Координаты ячейки не могут быть отрицательными.");
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => obj is Position p && X == p.X && Y == p.Y;
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"[{X}, {Y}]";
    }
}