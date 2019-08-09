using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IShip
    {
        int Size { get; }
        IList<IField> Coordinates { get; }
        bool IsSunk { get; }

        bool TryToShoot(int positionX, int positionY);
    }
}
