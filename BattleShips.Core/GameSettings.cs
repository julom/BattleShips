using System.Collections.Generic;

namespace BattleShips.Core
{
    public static class GameSettings
    {
        public static int BoardSizeX = 10;
        public static int BoardSizeY = 10;

        public static IList<int> ShipSizes = new List<int> {4, 5, 5};
    }
}
