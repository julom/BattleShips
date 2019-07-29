using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using BattleShips.Core.GameEntities.Factories;
using Microsoft.Extensions.DependencyInjection;
using BattleShips.Core.GameEntities.Utils.Abstract;
using System;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class Board_Tests
    {
        private static readonly IGameSettings _gameSettings;
        private static readonly IShipFactory _shipFactory;
        private static readonly IShipPositionsRandomizer _shipPositionsRandomizer;
        Board board;

        static Board_Tests()
        {
            IServiceProvider serviceProvider = DIContainersTestConfiguration.GetDIServiceProvider();
            _gameSettings = serviceProvider.GetService<IGameSettings>();
            _shipFactory = serviceProvider.GetService<IShipFactory>();
            _shipPositionsRandomizer = serviceProvider.GetService<IShipPositionsRandomizer>();
        }

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
            _gameSettings.ShipSizes = new List<int> { 2 };
            _gameSettings.BoardSizeX = 3;
            _gameSettings.BoardSizeY = 3;
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Board_PopulatesPlayerFields(bool[,] fields)
        {
            board = new Board(fields, _gameSettings, _shipFactory);

            Assert.IsNotNull(board.Fields);
            Assert.IsNotEmpty(board.Fields);
        }

        [Test]
        public void Board_PopulatesComputerFields()
        {
            board = new Board(_gameSettings, _shipFactory, _shipPositionsRandomizer);

            Assert.IsNotNull(board.Fields);
            Assert.IsNotEmpty(board.Fields);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void AreAllShipsSunk_AllShipsSunk_ReturnsTrue(bool[,] fields)
        {
            board = new Board(fields, _gameSettings, _shipFactory);

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
            board = new Board(fields, _gameSettings, _shipFactory);

            board.Shoot(1, 1);

            Assert.IsFalse(board.AreAllShipsSunk);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void DefineShipsPositions_PopulatesShips(bool[,] fields)
        {
            board = new Board(fields, _gameSettings, _shipFactory);

            board.DefineShipsPositions(fields);

            Assert.IsNotEmpty(board.Ships);
        }        

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtShipField_ReturnsTrue(bool[,] fields)
        {
            board = new Board(fields, _gameSettings, _shipFactory);
            var positionX = 1;
            var positionY = 1;

            var result = board.Shoot(positionX, positionY);

            Assert.IsTrue(result.IsShipHit);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtShipField_ChangesFieldTypeToShipHit(bool[,] fields)
        {
            board = new Board(fields, _gameSettings, _shipFactory);
            var positionX = 1;
            var positionY = 1;

            var result = board.Shoot(positionX, positionY);

            Assert.AreEqual(FieldTypes.ShipHit, board.Fields[positionX,positionY].FieldType);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtEmptyField_ReturnsFalse(bool[,] fields)
        {
            board = new Board(fields, _gameSettings, _shipFactory);
            var positionX = 0;
            var positionY = 0;

            var result = board.Shoot(positionX, positionY);

            Assert.IsFalse(result.IsShipHit);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Shoot_AtEmptyField_ChangesFieldTypeToMissedShot(bool[,] fields)
        {
            board = new Board(fields, _gameSettings, _shipFactory);
            var positionX = 0;
            var positionY = 0;

            var result = board.Shoot(positionX, positionY);

            Assert.AreEqual(FieldTypes.MissedShot, board.Fields[positionX, positionY].FieldType);
        }
    }
}
