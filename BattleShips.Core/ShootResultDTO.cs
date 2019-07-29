namespace BattleShips.Core.GameEntities.Abstract
{
    public class ShootResultDTO
    {
        public bool IsShipHit { get; set; }
        public bool IsShipSunk { get; set; }

        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
