using BattleShips.Core.Abstract;

namespace BattleShips.Core
{
    class Board : IBoard
    {
        public IShip[] Ships { get; private set; }

        public IField[,] Fields { get; private set; }

        public Board(IField[,] fields)
        {
            Fields = fields;
        }

        public void RandomizeShipsPositions()
        {
            throw new System.NotImplementedException();
        }

        public void DefineShipsPositions()
        {
            throw new System.NotImplementedException();
        }

        public bool Shoot(int positionX, int positionY)
        {
            throw new System.NotImplementedException();
        }
    }
}
