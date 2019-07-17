using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using NUnit.Framework;
using System;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class Field_Tests
    {
        Field _field;

        [Test]
        public void Field_FieldTypeIsSet([Values] FieldTypeEnum fieldType)
        {
            _field = new Field(fieldType);

            Assert.AreEqual(fieldType, _field.FieldType);
        }

        [Test]
        public void Shoot_ShootEmptyField_ReturnsFalse()
        {
            _field = new Field(FieldTypeEnum.Empty);

            var result = _field.Shoot();

            Assert.IsFalse(result);
        }

        [Test]
        public void Shoot_ShootShipField_ReturnsTrue()
        {
            _field = new Field(FieldTypeEnum.Ship);

            var result = _field.Shoot();

            Assert.IsTrue(result);
        }

        [Test]
        public void Shoot_ShootEmptyField_ChangesFieldTypeToMissedShot()
        {
            _field = new Field(FieldTypeEnum.Empty);

            _field.Shoot();

            Assert.AreEqual(FieldTypeEnum.MissedShot, _field.FieldType);
        }

        [Test]
        public void Shoot_ShootShipField_ChangesFieldTypeToShipHit()
        {
            _field = new Field(FieldTypeEnum.Ship);

            _field.Shoot();

            Assert.AreEqual(FieldTypeEnum.ShipHit, _field.FieldType);
        }

        [Test]
        public void Shoot_AlreadyShotField_ThrowsException([Values (FieldTypeEnum.MissedShot, FieldTypeEnum.ShipHit)] FieldTypeEnum fieldType)
        {
            _field = new Field(fieldType);

            TestDelegate action = () => _field.Shoot();

            Assert.Throws<GameLogicalException>(action);
        }
    }
}
