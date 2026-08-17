namespace Battleship
{
    // Требование 1.4 и 5: record Shot с автоматическим вычислением результата
    public record Shot(Board TargetBoard, Position Position, Ship HitShip)
    {
        // ИСПРАВЛЕНО: Добавлено свойство IsHit, чтобы код в Game.cs знал, рисовать X или O
        public bool IsHit => HitShip != null;

        public ShootResult Result
        {
            get
            {
                if (HitShip == null) return ShootResult.Miss;
                return HitShip.IsSunk ? ShootResult.Sunk : ShootResult.Hit;
            }
        }
    }
}