using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IShipCoordinatesValidator
    {
        bool Validate(IList<KeyValuePair<int, int>> coordinates);
    }
}
