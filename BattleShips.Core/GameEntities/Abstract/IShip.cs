using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IShip
    {
        int Size { get; }
        IList<IFieldPositionWrapper> Cooridnates { get; }
        bool IsSunk { get; }

        bool TryToShoot(int positionX, int positionY);
    }
}
