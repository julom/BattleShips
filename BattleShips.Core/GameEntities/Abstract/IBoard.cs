namespace BattleShips.Core.Abstract
{
    public interface IBoard
    {
        IShip[] Ships { get; }
        IField[,] Fields { get; }

        void RandomizeFields();
    }
}
