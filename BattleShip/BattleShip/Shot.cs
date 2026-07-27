using System;

namespace Battleship
{
    // ПУНКТ 3: Класс Shot для хранения истории
    public class Shot
    {
        public Board TargetBoard { get; }
        public Position Position { get; }
        public Ship HitShip { get; } // null, если промах

        public bool IsHit => HitShip != null;

        public Shot(Board targetBoard, Position position, Ship hitShip)
        {
            TargetBoard = targetBoard ?? throw new ArgumentNullException(nameof(targetBoard));
            Position = position ?? throw new ArgumentNullException(nameof(position));
            HitShip = hitShip; // может быть null
        }
    }
}