using BattleShips.Core.GameEntities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class Ship_Test
    {
        public static IList<KeyValuePair<int,int>> CreateCoordinates(params (int key, int value)[] pairs)
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
            List<KeyValuePair<int,int>> list = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < GameSettings.ShipSizes.Min() - 1; i++)
            {
                yield return new TestCaseData(list);
                list.AddRange(CreateCoordinates((0, i)));
            }
        }

        Ship _ship;

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void Ship_HasEqualCoordinatesCount(IList<KeyValuePair<int, int>> shipFields)
        {
            _ship = new Ship(shipFields);

            Assert.AreEqual(shipFields.Count,_ship.Cooridnates.Count);
        }

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void Ship_CalculatesSize(IList<KeyValuePair<int, int>> shipFields)
        {
            _ship = new Ship(shipFields);

            Assert.AreEqual(shipFields.Count, _ship.Size);
        }

        [Test]
        [TestCaseSource(nameof(WrongCoordinates))]
        public void Ship_HasCoordinatesNotInStraightLine_ThrowsException(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.Throws<GameLogicalException>(() => new Ship(shipFields), "Wrong coordinates, ships must be placed in one line without spaces");
        }

        [Test]
        [TestCaseSource(nameof(TooShortCoordinates))]
        public void Ship_HasStraightLineCoordinatesLessThanSmallestShipSize_ThrowsException(IList<KeyValuePair<int, int>> shipFields)
        {
            Assert.Throws<GameLogicalException>(() => new Ship(shipFields), "Too small number of coordinates, required at least " + GameSettings.ShipSizes.Min());
        }

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void IsSunk_HasAllSegmentsHit_ReturnsTrue(IList<KeyValuePair<int, int>> shipFields)
        {
            _ship = new Ship(shipFields);

            foreach (var field in shipFields)
            {
                _ship.TryToShoot(field.Key, field.Value);
            }

            Assert.IsTrue(_ship.IsSunk, "After hitting all segments, ship should be sunk");
        }

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void IsSunk_HasAtLeastOneSegmentSaved_ReturnsFalse(IList<KeyValuePair<int, int>> shipFields)
        {
            _ship = new Ship(shipFields);

            foreach (var field in shipFields.Skip(1))
            {
                _ship.TryToShoot(field.Key, field.Value);
            }

            Assert.IsFalse(_ship.IsSunk, "When not all segments are hit, ship should not be sunk");
        }

        [Test]
        public void TryToShoot_SegmentHit_CoordinateFieldChangesToFieldTypeHit()
        {
            var shipCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(0, 0) };
            var shootCoordinates = shipCoordinates;
            _ship = new Ship(shipCoordinates);

            _ship.TryToShoot(shootCoordinates.First().Key, shootCoordinates.First().Value);

            Assert.IsNotNull(_ship.Cooridnates.Single(x => x.Field.FieldType == FieldTypeEnum.ShipHit),
                "Field occupied by ship that is shot should change status to hit");
        }

        [Test]
        public void TryToShoot_SegmentHit_ReturnsTrue()
        {
            var shipCoordinates = new List<KeyValuePair<int, int>>() {new KeyValuePair<int, int>(0, 0)};
            var shootCoordinates = shipCoordinates;
            _ship = new Ship(shipCoordinates);

            var result = _ship.TryToShoot(shootCoordinates.First().Key, shootCoordinates.First().Value);

            Assert.IsTrue(result, "TryShoot should return true if shot is hit");
        }

        [Test]
        public void TryToShoot_SegmentSaved_CoordinateFieldChangesToFieldTypeMiss()
        {
            var shipCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(0, 0) };
            var shootCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(1, 1) };
            _ship = new Ship(shipCoordinates);

            _ship.TryToShoot(shootCoordinates.First().Key, shootCoordinates.First().Value);

            Assert.IsTrue(_ship.Cooridnates.All(x => x.Field.FieldType == FieldTypeEnum.Ship),
                "Field that is not shot should not change status");
        }

        [Test]
        public void TryToShoot_SegmentSaved_ReturnsFalse()
        {
            var shipCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(0, 0) };
            var shootCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(1, 1) };
            _ship = new Ship(shipCoordinates);

            var result = _ship.TryToShoot(shootCoordinates.First().Key, shootCoordinates.First().Value);

            Assert.IsFalse(result, "TryShoot should return false if shot is missed");
        }
    }
}
