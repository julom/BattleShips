namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IField
    {
        FieldTypeEnum FieldType { get; }

        void Shoot(bool success);  
    }
}
