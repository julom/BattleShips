namespace BattleShips.Core.Abstract
{
    public interface IField
    {
        FieldTypeEnum FieldType { get; }

        void Hit(bool success);  
    }
}
