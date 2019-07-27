using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Structs;

namespace BattleShips.Core.GameEntities
{
    public class ShipVectorsValidator : IShipVectorsValidator
    {
        public bool Validate(ShipVector vectorX, ShipVector vectorY)
        {
            return true;
        }
    }
}