using System;
using System.Collections.Generic;

namespace Battleship
{
    public class Board
    {
        public string OwnerName { get; }
        public int Size { get; }
        public List<Ship> Ships { get; }

        public Board(string ownerName, int size)
        {
            // ПУНКТ 1: Валидация доски
            if (string.IsNullOrWhiteSpace(ownerName))
                throw new ArgumentException("Имя владельца доски не может быть пустым.");
            
            if (size <= 0)
                throw new ArgumentException("Размер игрового поля должен быть больше нуля.");

            OwnerName = ownerName;
            Size = size;
            Ships = new List<Ship>();
        }

        public void AddShip(Ship ship)
        {
            if (ship == null) throw new ArgumentNullException(nameof(ship));

            // ПУНКТ 1: Проверка того, что корабль находится внутри поля (делается в Board)
            foreach (var cell in ship.Cells)
            {
                if (cell.X >= Size || cell.Y >= Size)
                {
                    throw new ArgumentException($"Корабль '{ship.Name}' выходит за границы игрового поля {Size}x{Size} на позиции {cell}.");
                }
            }
            Ships.Add(ship);
        }

        // ПУНКТ 4: Метод определяет конкретный корабль по переданной позиции
        public Ship FindShip(Position position)
        {
            if (position == null) return null;
            // Ищем первый корабль, список координат которого содержит искомую точку
            foreach (var ship in Ships)
            {
                if (ship.Cells.Contains(position))
                {
                    return ship;
                }
            }
            return null; // Если промах
        }
    }
}