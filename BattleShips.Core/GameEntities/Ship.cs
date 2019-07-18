using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;

namespace BattleShips.Core.GameEntities
{
    public class Ship : IShip
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

        public Ship(IList<KeyValuePair<int,int>> coordinates)
        {
            
        }

        public void TryToShoot(int positionX, int positionY)
        {
            throw new System.NotImplementedException();
        }
    }
}
