using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.Tests
{
    class TestGameSettings : IGameSettings
    {
        public int BoardSizeX { get; set; }
        public int BoardSizeY { get; set; }
        public IList<int> ShipSizes { get; set; }
    }
}
