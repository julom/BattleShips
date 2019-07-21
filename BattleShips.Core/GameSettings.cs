using System.Collections.Generic;

namespace BattleShips.Core
{
    public static class GameSettings
    {
        public const int BoardSizeX = 10;
        public const int BoardSizeY = 10;

        public static readonly IList<int> ShipSizes = new List<int> {4, 5, 5};
    }
}
