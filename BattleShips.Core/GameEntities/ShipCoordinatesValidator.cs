using System.Collections.Generic;
using BattleShips.Core.GameEntities.Abstract;

namespace BattleShips.Core.GameEntities
{
    public class ShipCoordinatesValidator : IShipCoordinatesValidator
    {
        const int AbsoluteMinimumNumberOfCoordinates = 2;
        public IList<KeyValuePair<int, int>> Coordinates => throw new System.NotImplementedException();

        public ShipCoordinatesValidator(IList<KeyValuePair<int, int>> coordinates)
        {

        }

        public bool Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
