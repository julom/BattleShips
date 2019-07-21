using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IBoard
    {
        IShip[] Ships { get; }
        IField[,] Fields { get; }

        void RandomizeShipsPositions(IList<int> shipSizes);
        IShip[] DefineShipsPositions(bool[,] fields);
        bool Shoot(int positionX, int positionY);
    }
}
