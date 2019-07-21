using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.Utils
{
    public static class CoordinatesUtils
    {
        public static IList<KeyValuePair<int, int>> CreateCoordinates(params (int key, int value)[] pairs)
        {
            var coordinates = new List<KeyValuePair<int, int>>(pairs.Length);

            foreach (var pair in pairs)
            {
                coordinates.Add(new KeyValuePair<int, int>(pair.key, pair.value));
            }
            return coordinates;
        }
    }
}
