using BattleShips.Core.GameEntities.Structs;

namespace BattleShips.Core.GameEntities.Validators.Abstract
{
    public interface IShipVectorsValidator
    {
        bool Validate(ShipVector vectorX, ShipVector vectorY);
    }
}
