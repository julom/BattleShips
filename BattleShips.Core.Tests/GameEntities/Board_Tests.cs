﻿using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Enums;
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

        [SetUp]
        public void InitializeGameSettings()
        {
            GameSettings.ShipSizes = new List<int> { 2 };
            GameSettings.BoardSizeX = 3;
            GameSettings.BoardSizeY = 3;
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Board_PopulatesFields(bool[,] fields)
        {
            board = new Board(fields);

            Assert.IsNotNull(board.Fields);
            Assert.IsNotEmpty(board.Fields);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void AreAllShipsSunk_AllShipsSunk_ReturnsTrue(bool[,] fields)
        {
            board = new Board(fields);

            board.Shoot(1, 1);
            board.Shoot(1, 2);
            board.Shoot(2, 1);
            board.Shoot(2, 2);

            Assert.IsTrue(board.AreAllShipsSunk);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void AreAllShipsSunk_NoShipsSunk_ReturnsFalse(bool[,] fields)
        {
            board = new Board(fields);

            board.Shoot(1, 1);

            Assert.IsFalse(board.AreAllShipsSunk);
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
        [Repeat(10)]
        public void RandomizeShipsPositions_PopulatesShips()
        {
            board = new Board();

            Assert.IsNotEmpty(board.Ships);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtShipField_ReturnsTrue(bool[,] fields)
        {
            board = new Board(fields);
            var positionX = 1;
            var positionY = 1;

            var result = board.Shoot(positionX, positionY);

            Assert.IsTrue(result.IsShipHit);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtShipField_ChangesFieldTypeToShipHit(bool[,] fields)
        {
            board = new Board(fields);
            var positionX = 1;
            var positionY = 1;

            var result = board.Shoot(positionX, positionY);

            Assert.AreEqual(FieldTypes.ShipHit, board.Fields[positionX,positionY].FieldType);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtEmptyField_ReturnsFalse(bool[,] fields)
        {
            board = new Board(fields);
            var positionX = 0;
            var positionY = 0;

            var result = board.Shoot(positionX, positionY);

            Assert.IsFalse(result.IsShipHit);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtEmptyField_ChangesFieldTypeToMissedShot(bool[,] fields)
        {
            board = new Board(fields);
            var positionX = 0;
            var positionY = 0;

            var result = board.Shoot(positionX, positionY);

            Assert.AreEqual(FieldTypes.MissedShot, board.Fields[positionX, positionY].FieldType);
        }
    }
}
