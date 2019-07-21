using BattleShips.Core.GameEntities.Enums;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IField
    {
        int PositionX { get; }
        int PositionY { get; }

        FieldTypes FieldType { get; }

        bool Shoot();  
    }
}
