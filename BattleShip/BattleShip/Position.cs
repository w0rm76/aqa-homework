namespace Battleship;

public class Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        // ПУНКТ 1: Проверка на отрицательные координаты
        if (x < 0 || y < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "Координаты ячейки не могут быть отрицательными.");
        }
        X = x;
        Y = y;
    }

    public override bool Equals(object obj) => obj is Position p && X == p.X && Y == p.Y;
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"[{X}, {Y}]";
}