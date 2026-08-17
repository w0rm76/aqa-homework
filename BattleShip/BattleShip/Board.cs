using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship
{
    public class Board
    {
        public string OwnerName { get; }
        public int Size { get; }
        public List<Ship> Ships { get; } = new List<Ship>();

        public Board(string ownerName, int size)
        {
            if (string.IsNullOrWhiteSpace(ownerName)) throw new ArgumentException("Имя владельца пустое.");
            if (size <= 0) throw new ArgumentException("Размер поля должен быть больше 0.");
            
            OwnerName = ownerName;
            Size = size;
        }

        public void AddShip(Ship ship)
        {
            if (ship == null) throw new ArgumentNullException(nameof(ship));

            // Проверка выхода корабля за пределы поля в классе Board (Занятие 6 / Пункт 1)
            foreach (var cell in ship.Cells)
            {
                if (cell.X >= Size || cell.Y >= Size)
                    throw new ArgumentException($"Корабль '{ship.Name}' выходит за границы игрового поля {Size}x{Size}.");
            }
            Ships.Add(ship);
        }

        // Занятие 6 / Пункт 4: Метод поиска конкретного корабля по позиции
        public Ship FindShip(Position position)
        {
            return Ships.FirstOrDefault(s => s.IsOnPosition(position));
        }
    }
}