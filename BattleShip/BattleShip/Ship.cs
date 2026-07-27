namespace Battleship;

public class Ship
{
    public string Name { get; }
    public List<Position> Cells { get; }

    public Ship(string name, List<Position> cells)
    {
        // ПУНКТ 1: Валидация параметров корабля
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя корабля должно быть заполнено.", nameof(name));
            
        if (cells == null || cells.Count == 0)
            throw new ArgumentException("Корабль должен занимать как минимум одну ячейку.", nameof(cells));

        Name = name;
        Cells = cells;
    }
}