using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;

namespace BattleShips.Core.GameEntities
{
    public class Ship : IShip
    {
        public int Size { get; private set; }
        public IList<IFieldPositionWrapper> Cooridnates { get; private set; }

        public bool IsSunk
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Ship(IList<KeyValuePair<int, int>> coordinates)
        {
            
        }

        public bool TryToShoot(int positionX, int positionY)
        {
            throw new System.NotImplementedException();
        }
    }
}
