using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IShip
    {
        int Size { get; }
        IField[,] Cooridnates { get; }
        bool IsSunk { get; }

        void TryToShoot(int positionX, int positionY);
    }
}
