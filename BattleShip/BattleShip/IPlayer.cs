namespace Battleship
{
    // Требование 1.3: Интерфейс IShooter
    public interface IShooter
    {
        Shot Shoot(Board targetBoard);
    }

    // Требование 1.3: Интерфейс IPlayer
    public interface IPlayer
    {
        string Name { get; }
        Board MyBoard { get; }
    }
}