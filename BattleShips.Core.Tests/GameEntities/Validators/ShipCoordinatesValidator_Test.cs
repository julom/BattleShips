using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities;
using BattleShips.Core.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class ShipCoordinatesValidator_Test
    {
        ShipCoordinatesValidator validator = new ShipCoordinatesValidator();


        public static IEnumerable<TestCaseData> ProperCoordinates()
        {
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((10, 10), (10, 11), (10, 12), (10, 13)));
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((10, 10), (11, 10), (12, 10), (13, 10)));
        }

        public static IEnumerable<TestCaseData> WrongCoordinates()
        {
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((9, 10), (10, 11), (10, 12), (10, 13)));
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((10, 9), (11, 10), (12, 10), (13, 10)));
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((9, 9), (11, 10), (12, 10), (13, 10)));
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((9, 10), (11, 10), (12, 10), (13, 10)));
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((9, 10), (11, 10), (12, 10), (13, 10)));
        }
        public static IEnumerable<TestCaseData> TooShortCoordinates()
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < GameSettings.ShipSizes.Min() - 1; i++)
            {
                yield return new TestCaseData(list);
                list.AddRange(CoordinatesUtils.CreateCoordinates((0, i)));
            }
        }

        public static IEnumerable<TestCaseData> TooBigCoordinates()
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < GameSettings.ShipSizes.Max() + 1; i++)
            {
                list.AddRange(CoordinatesUtils.CreateCoordinates((0, i)));
            }
            yield return new TestCaseData(list);
        }

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void Ship_HasProperCoordinates_ReturnsTrue(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.IsTrue(validator.Validate(shipFields), "Proper coordinates not passed");
        }

        [Test]
        [TestCaseSource(nameof(WrongCoordinates))]
        public void Ship_HasCoordinatesNotInStraightLine_ReturnsFalse(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.IsFalse(validator.Validate(shipFields), "Wrong coordinates, ships must be placed in one line without spaces");
        }

        [Test]
        [TestCaseSource(nameof(TooShortCoordinates))]
        public void Ship_HasStraightLineCoordinatesLessThanSmallestShipSize_ReturnsFalse(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.IsFalse(validator.Validate(shipFields), "Too small number of coordinates, required at least " + GameSettings.ShipSizes.Min());
        }

        [Test]
        [TestCaseSource(nameof(TooBigCoordinates))]
        public void Ship_HasStraightLineCoordinatesMoreThanBiggestShipSize_ReturnsFalse(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.IsFalse(validator.Validate(shipFields), "Too big number of coordinates, required max " + GameSettings.ShipSizes.Max());
        }
    }
}
