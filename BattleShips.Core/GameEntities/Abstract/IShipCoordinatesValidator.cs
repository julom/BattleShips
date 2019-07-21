using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IShipCoordinatesValidator
    {
        IList<KeyValuePair<int, int>> Coordinates { get; }

        bool Validate();
    }
}
