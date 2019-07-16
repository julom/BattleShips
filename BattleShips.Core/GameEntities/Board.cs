using BattleShips.Core.Abstract;

namespace BattleShips.Core
{
    class Board : IBoard
    {
        public IShip[] Ships { get; private set; }

        public IField[,] Fields { get; private set; }

        public Board()
        {

        }

        public void RandomizeFields()
        {
            throw new System.NotImplementedException();
        }
    }
}
