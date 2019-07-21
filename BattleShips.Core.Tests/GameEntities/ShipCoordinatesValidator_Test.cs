using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.Tests.GameEntities
{
    public class ShipCoordinatesValidator_Test
    {
        ShipCoordinatesValidator validator;

        public static IList<KeyValuePair<int, int>> CreateCoordinates(params (int key, int value)[] pairs)
        {
            var coordinates = new List<KeyValuePair<int, int>>(pairs.Length);

            foreach (var pair in pairs)
            {
                coordinates.Add(new KeyValuePair<int, int>(pair.key, pair.value));
            }
            return coordinates;
        }


        public static IEnumerable<TestCaseData> ProperCoordinates()
        {
            yield return new TestCaseData(CreateCoordinates((10, 10), (10, 11), (10, 12), (10, 13)));
            yield return new TestCaseData(CreateCoordinates((10, 10), (11, 10), (12, 10), (13, 10)));
        }

        public static IEnumerable<TestCaseData> WrongCoordinates()
        {
            yield return new TestCaseData(CreateCoordinates((9, 10), (10, 11), (10, 12), (10, 13)));
            yield return new TestCaseData(CreateCoordinates((10, 9), (11, 10), (12, 10), (13, 10)));
            yield return new TestCaseData(CreateCoordinates((9, 9), (11, 10), (12, 10), (13, 10)));
            yield return new TestCaseData(CreateCoordinates((9, 10), (11, 10), (12, 10), (13, 10)));
            yield return new TestCaseData(CreateCoordinates((9, 10), (11, 10), (12, 10), (13, 10)));
        }
        public static IEnumerable<TestCaseData> TooShortCoordinates()
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < GameSettings.ShipSizes.Min() - 1; i++)
            {
                yield return new TestCaseData(list);
                list.AddRange(CreateCoordinates((0, i)));
            }
        }

        public static IEnumerable<TestCaseData> TooBigCoordinates()
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < GameSettings.ShipSizes.Max() + 1; i++)
            {
                list.AddRange(CreateCoordinates((0, i)));
            }
            yield return new TestCaseData(list);
        }


        [Test]
        [TestCaseSource(nameof(WrongCoordinates))]
        public void Ship_HasCoordinatesNotInStraightLine_ThrowsException(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.Throws<GameArgumentException>(() => new ShipCoordinatesValidator(shipFields), "Wrong coordinates, ships must be placed in one line without spaces");
        }

        [Test]
        [TestCaseSource(nameof(TooShortCoordinates))]
        public void Ship_HasStraightLineCoordinatesLessThanSmallestShipSize_ThrowsException(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.Throws<GameArgumentException>(() => new ShipCoordinatesValidator(shipFields), "Too small number of coordinates, required at least " + GameSettings.ShipSizes.Min());
        }

        [Test]
        [TestCaseSource(nameof(TooShortCoordinates))]
        public void Ship_HasStraightLineCoordinatesMoreThanBiggestShipSize_ThrowsException(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.Throws<GameArgumentException>(() => new ShipCoordinatesValidator(shipFields), "Too big number of coordinates, required max " + GameSettings.ShipSizes.Max());
        }
    }
}
