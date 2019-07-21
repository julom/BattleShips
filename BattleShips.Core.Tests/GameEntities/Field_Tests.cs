using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Enums;
using NUnit.Framework;
using System;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class Field_Tests
    {
        Field _field;

        [Test]
        public void Field_FieldTypeIsSet([Values] FieldTypes fieldType)
        {
            _field = new Field(fieldType, 0, 0);

            Assert.AreEqual(fieldType, _field.FieldType);
        }

        [Test]
        public void Shoot_ShootEmptyField_ReturnsFalse()
        {
            _field = new Field(FieldTypes.Empty, 0, 0);

            var result = _field.Shoot();

            Assert.IsFalse(result);
        }

        [Test]
        public void Shoot_ShootShipField_ReturnsTrue()
        {
            _field = new Field(FieldTypes.Ship, 0, 0);

            var result = _field.Shoot();

            Assert.IsTrue(result);
        }

        [Test]
        public void Shoot_ShootEmptyField_ChangesFieldTypeToMissedShot()
        {
            _field = new Field(FieldTypes.Empty, 0, 0);

            _field.Shoot();

            Assert.AreEqual(FieldTypes.MissedShot, _field.FieldType);
        }

        [Test]
        public void Shoot_ShootShipField_ChangesFieldTypeToShipHit()
        {
            _field = new Field(FieldTypes.Ship, 0, 0);

            _field.Shoot();

            Assert.AreEqual(FieldTypes.ShipHit, _field.FieldType);
        }

        [Test]
        public void Shoot_AlreadyShotField_ThrowsException([Values (FieldTypes.MissedShot, FieldTypes.ShipHit)] FieldTypes fieldType)
        {
            _field = new Field(fieldType, 0, 0);

            TestDelegate action = () => _field.Shoot();

            Assert.Throws<GameLogicalException>(action);
        }

        [Test]
        public void Shoot_NotExpectedFieldType_ThrowsException([Values(-1)] FieldTypes fieldType)
        {
            _field = new Field(fieldType, 0, 0);

            TestDelegate action = () => _field.Shoot();

            Assert.Throws<GameArgumentException>(action);
        }
    }
}
