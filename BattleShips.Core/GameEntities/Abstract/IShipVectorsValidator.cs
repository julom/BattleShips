using BattleShips.Core.GameEntities.Structs;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IShipVectorsValidator
    {
        bool Validate(ShipVector vectorX, ShipVector vectorY);
    }
}
