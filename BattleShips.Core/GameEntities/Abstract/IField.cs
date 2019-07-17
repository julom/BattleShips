namespace BattleShips.Core.Abstract
{
    public interface IField
    {
        FieldTypeEnum FieldType { get; }

        void Shoot(bool success);  
    }
}
