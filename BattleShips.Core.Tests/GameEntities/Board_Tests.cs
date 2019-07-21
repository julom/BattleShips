using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class Board_Tests
    {
        Board board;
        Mock<IShip> mockShip = new Mock<IShip>();

        public static IEnumerable<TestCaseData> FieldCoordinates()
        {
            yield return new TestCaseData(new bool[,] { 
                { false, false, false }, 
                { false, true, false },
                { false, true, false }
            });
            yield return new TestCaseData(new bool[,] {
                { false, false, false },
                { false, true, true },
                { false, false, false }
            });
        }


        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Board_PopulatesFields(bool[,] fields)
        {
            board = new Board(fields);

            Assert.IsNotEmpty(board.Fields);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void DefineShipsPositions_PopulatesShips(bool[,] fields)
        {
            board = new Board(fields);

            board.DefineShipsPositions(fields);

            Assert.IsNotEmpty(board.Ships);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void RandomizeShipsPositions_PopulatesShips(bool[,] fields)
        {
            board = new Board(fields);

            board.RandomizeShipsPositions(GameSettings.ShipSizes);

            Assert.IsNotEmpty(board.Ships);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtShipField_ReturnsTrue(bool[,] fields)
        {
            board = new Board(fields);
            var positionX = 1;
            var positionY = 1;
            mockShip.Setup(x => x.TryToShoot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = board.Shoot(positionX, positionY);

            Assert.IsTrue(result);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtShipField_ChangesFieldTypeToShipHit(bool[,] fields)
        {
            board = new Board(fields);
            var positionX = 1;
            var positionY = 1;
            mockShip.Setup(x => x.TryToShoot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = board.Shoot(positionX, positionY);

            Assert.AreEqual(FieldTypeEnum.ShipHit, board.Fields[positionX,positionY].FieldType);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtEmptyField_ReturnsFalse(bool[,] fields)
        {
            board = new Board(fields);
            var positionX = 0;
            var positionY = 0;
            mockShip.Setup(x => x.TryToShoot(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var result = board.Shoot(positionX, positionY);

            Assert.IsFalse(result);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtEmptyField_ChangesFieldTypeToMissedShot(bool[,] fields)
        {
            board = new Board(fields);
            var positionX = 0;
            var positionY = 0;
            mockShip.Setup(x => x.TryToShoot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = board.Shoot(positionX, positionY);

            Assert.AreEqual(FieldTypeEnum.MissedShot, board.Fields[positionX, positionY].FieldType);
        }
    }
}
