using BattleShips.Core.GameEntities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core.Exceptions;
using Moq;
using BattleShips.Core.Utils;
using BattleShips.Core.GameEntities.Enums;
using BattleShips.Core.GameEntities.Validators.Abstract;
using BattleShips.Core.GameEntities.Structs;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class Ship_Tests
    {
        public static IEnumerable<TestCaseData> ProperCoordinates()
        {
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((10, 10), (10, 11), (10, 12), (10, 13)));
            yield return new TestCaseData(CoordinatesUtils.CreateCoordinates((10, 10), (11, 10), (12, 10), (13, 10)));
        }

        public static IEnumerable<TestCaseData> ProperVectors()
        {
            yield return new TestCaseData(new ShipVector(10, 10), new ShipVector(10, 13));
            yield return new TestCaseData(new ShipVector(10, 10), new ShipVector(13, 10));
        }


        Ship _ship;
        Mock<IShipCoordinatesValidator> mockShipCoordinatesValidator;
        Mock<IShipVectorsValidator> mockShipVectorsValidator;

        [SetUp]
        public void Initialize()
        {
            mockShipCoordinatesValidator = new Mock<IShipCoordinatesValidator>();
            mockShipCoordinatesValidator.Setup(x => x.Validate(It.IsAny<IList<KeyValuePair<int, int>>>())).Returns(true);
            mockShipVectorsValidator = new Mock<IShipVectorsValidator>();
            mockShipVectorsValidator.Setup(x => x.Validate(It.IsAny<ShipVector>(), It.IsAny<ShipVector>())).Returns(true);
        }

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void Ship_HasEqualCoordinatesCount(IList<KeyValuePair<int, int>> shipFields)
        {
            _ship = new Ship(shipFields, mockShipCoordinatesValidator.Object);

            Assert.AreEqual(shipFields.Count, _ship.Coordinates.Count);
        }

        [Test]
        [TestCaseSource(nameof(ProperVectors))]
        public void Ship_HasEqualCoordinatesCount(ShipVector vectorX, ShipVector vectorY)
        {
            _ship = new Ship(vectorX, vectorY, mockShipVectorsValidator.Object);

            Assert.AreEqual(vectorX.Size * vectorY.Size, _ship.Coordinates.Count);
        }

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void Ship_NotPassedCoordinatesValidation(IList<KeyValuePair<int, int>> shipFields)
        {
            mockShipCoordinatesValidator.Setup(x => x.Validate(It.IsAny<IList<KeyValuePair<int, int>>>())).Returns(false);

            Assert.Throws<GameArgumentException>(() => new Ship(shipFields, mockShipCoordinatesValidator.Object), 
                "Validator should not allow to create Ship if coordinates are wrong");
        }

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void Ship_CalculatesSize(IList<KeyValuePair<int, int>> shipFields)
        {
            _ship = new Ship(shipFields, mockShipCoordinatesValidator.Object);

            Assert.AreEqual(shipFields.Count, _ship.Size, "Ship size should count all ship segments");
        }

        [Test]
        [TestCaseSource(nameof(ProperCoordinates))]
        public void IsSunk_HasAllSegmentsHit_ReturnsTrue(IList<KeyValuePair<int, int>> shipFields)
        {
            _ship = new Ship(shipFields, mockShipCoordinatesValidator.Object);

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
            _ship = new Ship(shipFields, mockShipCoordinatesValidator.Object);

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
            _ship = new Ship(shipCoordinates, mockShipCoordinatesValidator.Object);

            _ship.TryToShoot(shootCoordinates.First().Key, shootCoordinates.First().Value);

            Assert.IsNotNull(_ship.Coordinates.Single(x => x.FieldType == FieldTypes.ShipHit),
                "Field occupied by ship that is shot should change status to hit");
        }

        [Test]
        public void TryToShoot_SegmentHit_ReturnsTrue()
        {
            var shipCoordinates = new List<KeyValuePair<int, int>>() {new KeyValuePair<int, int>(0, 0)};
            var shootCoordinates = shipCoordinates;
            _ship = new Ship(shipCoordinates, mockShipCoordinatesValidator.Object);

            var result = _ship.TryToShoot(shootCoordinates.First().Key, shootCoordinates.First().Value);

            Assert.IsTrue(result, "TryShoot should return true if shot is hit");
        }

        [Test]
        public void TryToShoot_SegmentSaved_CoordinateFieldChangesToFieldTypeMiss()
        {
            var shipCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(0, 0) };
            var shootCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(1, 1) };
            _ship = new Ship(shipCoordinates, mockShipCoordinatesValidator.Object);

            _ship.TryToShoot(shootCoordinates.First().Key, shootCoordinates.First().Value);

            Assert.IsTrue(_ship.Coordinates.All(x => x.FieldType == FieldTypes.Ship),
                "Field that is not shot should not change status");
        }

        [Test]
        public void TryToShoot_SegmentSaved_ReturnsFalse()
        {
            var shipCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(0, 0) };
            var shootCoordinates = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(1, 1) };
            _ship = new Ship(shipCoordinates, mockShipCoordinatesValidator.Object);

            var result = _ship.TryToShoot(shootCoordinates.First().Key, shootCoordinates.First().Value);

            Assert.IsFalse(result, "TryShoot should return false if shot is missed");
        }
    }
}
