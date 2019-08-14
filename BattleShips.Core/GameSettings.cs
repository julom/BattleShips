using System.Collections.Generic;

namespace BattleShips.Core
{
    public class GameSettings : IGameSettings
    {
        public const int BoardSizeXDefault = 10;
        public const int BoardSizeYDefault = 10;
        public const int ShipsCountDefault = 3;
        public static readonly int[] ShipSizesDefault = new int[ShipsCountDefault] { 5, 4, 4 };

        public int BoardSizeX { get; set; } = BoardSizeXDefault;
        public int BoardSizeY { get; set; } = BoardSizeYDefault;

        public IList<int> ShipSizes { get; set; } = new List<int>(ShipSizesDefault);
    }
}
