using System.Collections.Generic;

namespace BattleShips.Core
{
    public class GameSettings : IGameSettings
    {
        public int BoardSizeX { get => 10; set{} }
        public int BoardSizeY { get => 10; set{} }

        public IList<int> ShipSizes { get => new List<int> {5, 4, 4}; set{} }
    }
}
