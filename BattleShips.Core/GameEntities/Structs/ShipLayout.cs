using System;
using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Structs
{
    public struct ShipLayout
    {
        public ShipVector VectorX { get; }
        public ShipVector VectorY { get; }
        public IEnumerable<Coordinate> Values
        {
            get
            {
                foreach (var positionX in VectorX.Values)
                {
                    foreach (var positionY in VectorY.Values)
                    {
                        yield return new Coordinate(positionX, positionY);
                    }
                }
            }
        }

        public ShipLayout(ShipVector vectorX, ShipVector vectorY)
        {
            VectorX = vectorX;
            VectorY = vectorY;
        }

        public ShipLayout(int fromX, int toX, int fromY, int toY)
        {
            VectorX = new ShipVector(fromX, toX);
            VectorY = new ShipVector(fromY, toY);
        }

        public ShipLayout(Coordinate coordinateFrom, Coordinate coordinateTo)
        {
            VectorX = new ShipVector(coordinateFrom.PositionX, coordinateTo.PositionX);
            VectorY = new ShipVector(coordinateFrom.PositionY, coordinateTo.PositionY);
        }
    }
}
