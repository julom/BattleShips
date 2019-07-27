using BattleShips.Core.GameEntities.Enums;
using BattleShips.Core.GameEntities.Structs;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IField
    {
        Coordinate Position { get; }
        int PositionX { get; }
        int PositionY { get; }

        FieldTypes FieldType { get; }

        bool IsShipField { get; }

        bool Shoot();  
    }
}
