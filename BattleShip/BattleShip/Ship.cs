using System;
using System.Collections.Generic;

namespace Battleship
{
    // Требование 1.2: Абстрактный класс Ship
    public abstract class Ship
    {
        public string Name { get; }
        public int Length { get; }
        public List<Position> Cells { get; protected set; } = new List<Position>();

        // ПУНКТ 5: Коллекция выстрелов, попавших в этот корабль
        public List<Shot> ShipHits { get; } = new List<Shot>();

        // ПУНКТ 5: Свойство, определяющее потоплен ли корабль
        public bool IsSunk => ShipHits.Count >= Length;

        protected Ship(string name, int length)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Имя корабля пустое.");
            if (length <= 0) throw new ArgumentException("Длина корабля должна быть больше нуля.");
            Name = name;
            Length = length;
        }

        public bool IsOnPosition(Position position)
        {
            return Cells.Contains(position);
        }

        // ПУНКТ 2: Абстрактный метод для проверки пересечения кораблей
        public abstract bool IntersectsWith(Ship other);
    }
}