using BattleShips.Core.GameEntities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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

        Ship _ship;
        
        [Test]
        [TestCaseSource("ProperCoordinates")]
        public void Ship_HasCoordinates(IList<KeyValuePair<int, int>> shipFields)
        {
            _ship = new Ship(shipFields);

            Assert.Fail();
        }

        [Test]
        public void Ship_CalculatesSize()
        {
            Assert.Fail();
        }

        [Test]
        public void Ship_HasCoordinatesNotInStraightLine_ThrowsException()
        {
            Assert.Fail();
        }

        [Test]
        public void Ship_HasStraightLineCoordinatesLessThanSmallestShipSize_ThrowsException()
        {
            Assert.Fail();
        }

        [Test]
        public void IsSunk_HasAllSegmentsHit_ReturnsTrue()
        {
            Assert.Fail();
        }

        [Test]
        public void IsSunk_HasAtLeastOneSegmentSaved_ReturnsFalse()
        {
            Assert.Fail();
        }

        [Test]
        public void TryToShoot_SegmentHit_CoordinateFieldExecutesShootWithTrueResult()
        {
            Assert.Fail();
        }

        [Test]
        public void TryToShoot_SegmentHit_ReturnsTrue()
        {
            Assert.Fail();
        }

        [Test]
        public void TryToShoot_SegmentSaved_CoordinateFieldExecutesShootWithFalseResult()
        {
            Assert.Fail();
        }

        [Test]
        public void TryToShoot_SegmentSaved_ReturnsFalse()
        {
            Assert.Fail();
        }
    }
}
