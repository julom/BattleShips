using System;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core.GameEntities.Validators.Abstract;

namespace BattleShips.Core.GameEntities.Validators
{
    public class ShipCoordinatesValidator : IShipCoordinatesValidator
    {
        const int AbsoluteMinimumNumberOfCoordinates = 2;

        private readonly IGameSettings _gameSettings;

        public ShipCoordinatesValidator(IGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public bool Validate(IList<KeyValuePair<int, int>> coordinates)
        {
            if (ValidateCoordinatesNumber(coordinates))
            {
                var rows = coordinates.Select(x => x.Key).ToArray();
                int rowsDifferByOne = GetNumberOfValuesDifferingByOne(rows);

                var columns = coordinates.Select(x => x.Value).ToArray();
                var columnsDifferByOne = GetNumberOfValuesDifferingByOne(columns);

                if ((rowsDifferByOne == coordinates.Count - 1 && columnsDifferByOne == 0) ||
                    (rowsDifferByOne == 0 && columnsDifferByOne == coordinates.Count - 1))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ValidateCoordinatesNumber(IList<KeyValuePair<int, int>> coordinates)
        {
            if (coordinates.Count < AbsoluteMinimumNumberOfCoordinates ||
                coordinates.Count < _gameSettings.ShipSizes.Min() ||
                coordinates.Count > _gameSettings.ShipSizes.Max())
            {
                return false;
            }
            return true;
        }

        private static int GetNumberOfValuesDifferingByOne(int[] values)
        {
            int valuesDifferByOne = 0;
            for (int i = 0; i < values.Length - 1; i++)
            {
                var nextElementBiggerByOne = values[i] + 1 == values[i + 1];
                valuesDifferByOne += Convert.ToInt32(nextElementBiggerByOne);
            }
            return valuesDifferByOne;
        }
    }
}
