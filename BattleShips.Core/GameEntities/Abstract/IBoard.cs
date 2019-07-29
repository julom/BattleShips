using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IBoard
    {
        IShip[] Ships { get; }
        IField[,] Fields { get; }
        bool AreAllShipsSunk { get; }

        IShip[] DefineShipsPositions(bool[,] fields);
        ShootResultDTO Shoot(int positionX, int positionY);
    }
}
