using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.GameEntities.Structs
{
    public struct ShipVector
    {
        public int From { get; }
        public int To { get; }
        public int Size => Values.Count();
        public IEnumerable<int> Values
        {
            get
            {
                for (int i = From; i <= To; i++)
                {
                    yield return i;
                }
            }
        }

        public ShipVector(int from, int to)
        {
            From = Math.Min(from, to);
            To = Math.Max(from, to);
        }
    }
}
