using System;
using System.Collections.Generic;

namespace Battleship
{
    // Требование 1.2: Наследник для горизонтального расположения
    public class HorizontalShip : Ship
    {
        public HorizontalShip(string name, int length, Position startPosition) : base(name, length)
        {
            for (int i = 0; i < length; i++)
            {
                Cells.Add(new Position(startPosition.X + i, startPosition.Y));
            }
        }

        // ПУНКТ 2: Переопределение метода пересечения
        public override bool IntersectsWith(Ship other)
        {
            if (other == null) return false;
            foreach (var cell in Cells)
            {
                if (other.IsOnPosition(cell)) return true; // Найдена общая клетка
            }
            return false;
        }
    }
}