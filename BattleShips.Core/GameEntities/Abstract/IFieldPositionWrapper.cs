namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IFieldPositionWrapper
    {
        IField Field { get; }

        int PositionX { get; }
        int PositionY { get; }
    }
}