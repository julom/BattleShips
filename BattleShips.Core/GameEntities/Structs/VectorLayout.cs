using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Structs
{
    public struct VectorLayout
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

        public VectorLayout(ShipVector vectorX, ShipVector vectorY)
        {
            VectorX = vectorX;
            VectorY = vectorY;
        }

        public VectorLayout(int fromX, int toX, int fromY, int toY)
        {
            VectorX = new ShipVector(fromX, toX);
            VectorY = new ShipVector(fromY, toY);
        }
    }
}
