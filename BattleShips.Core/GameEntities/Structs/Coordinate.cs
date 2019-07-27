using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.GameEntities.Structs
{
    public struct Coordinate
    {
        public int PositionX { get; }
        public int PositionY { get; }

        public Coordinate(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}
