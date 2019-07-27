using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Validators.Abstract
{
    public interface IShipCoordinatesValidator
    {
        bool Validate(IList<KeyValuePair<int, int>> coordinates);
    }
}
