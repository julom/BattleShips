using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core
{
    public interface IGameSettings
    {
        int BoardSizeX { get; set; }
        int BoardSizeY { get; set; }
        IList<int> ShipSizes { get; set; }
    }
}
