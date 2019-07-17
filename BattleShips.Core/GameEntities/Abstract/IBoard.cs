namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IBoard
    {
        IShip[] Ships { get; }
        IField[,] Fields { get; }

        void RandomizeShipsPositions();
        void DefineShipsPositions();
        bool Shoot(int positionX, int positionY);
    }
}
