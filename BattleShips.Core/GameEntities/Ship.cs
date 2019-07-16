using BattleShips.Core.Abstract;

namespace BattleShips.Core
{
    class Ship : IShip
    {
        public int Size { get; private set; }
        public IField[,] Cooridnates { get; private set; }

        public bool IsSunk
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Ship(IField[,] coordinates)
        {
            Cooridnates = coordinates;
        }

        public void HitSegment(int x, int y)
        {
            throw new System.NotImplementedException();
        }
    }
}
