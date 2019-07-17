using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.Abstract
{
    public interface IShip
    {
        int Size { get; }
        IField[,] Cooridnates { get; }
        bool IsSunk { get; }

        void HitSegment(int positionX, int positionY);
    }
}
