using System.Collections.Generic;

namespace BattleShips.Core.Tests
{
    class TestGameSettings : IGameSettings
    {
        public int BoardSizeX { get; set; } = 10;
        public int BoardSizeY { get; set; } = 10;
        public IList<int> ShipSizes { get; set; } = new[] { 5, 4, 4 };
    }
}
