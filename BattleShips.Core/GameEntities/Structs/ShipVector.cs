using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.GameEntities.Structs
{
    public struct ShipVector
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Size { get { return Values.Count(); } }
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
            From = from;
            To = to;
        }
    }
}
