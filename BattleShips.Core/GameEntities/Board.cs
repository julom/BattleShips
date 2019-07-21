using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;

namespace BattleShips.Core.GameEntities
{
    public class Board : IBoard
    {
        public IShip[] Ships { get; private set; }

        public IField[,] Fields { get; private set; }

        public Board(bool[,] fields)
        {
            DefineShipsPositions(fields);
        }

        public void RandomizeShipsPositions(IList<int> shipSizes)
        {
            throw new System.NotImplementedException();
        }

        public IShip[] DefineShipsPositions(bool[,] fields)
        {
            throw new System.NotImplementedException();
        }

        public bool Shoot(int positionX, int positionY)
        {
            throw new System.NotImplementedException();
        }
    }
}
