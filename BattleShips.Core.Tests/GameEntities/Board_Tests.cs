using Moq;
using System;
using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using BattleShips.Core.GameEntities.Utils.Abstract;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Validators.Abstract;
using BattleShips.Tests;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class Board_Tests
    {
        private static readonly IGameSettings _gameSettings;
        private static readonly IShipPositionsRandomizer _shipPositionsRandomizer;
        private static readonly IShipsGroupValidator _shipsGroupValidator;
        Board board;
        Mock<IShip> _mockShip;

        static Board_Tests()
        {
            IServiceProvider serviceProvider = DIContainersTestConfiguration.GetDIServiceProvider();
            _gameSettings = serviceProvider.GetService<IGameSettings>();
            _shipPositionsRandomizer = serviceProvider.GetService<IShipPositionsRandomizer>();
            _shipsGroupValidator = serviceProvider.GetService<IShipsGroupValidator>();
        }

        public static IEnumerable<TestCaseData> ShipsCoordinates()
        {
            yield return new TestCaseData(new List<IField>() {
                    new Field(FieldTypes.Ship, 1, 1),
                    new Field(FieldTypes.Ship, 1, 2)
                });

            yield return new TestCaseData(new List<IField>() {
                    new Field(FieldTypes.Ship, 1, 1),
                    new Field(FieldTypes.Ship, 2, 1)
                });
        }

        [SetUp]
        public void InitializeGameSettings()
        {
            _mockShip = new Mock<IShip>();
            _mockShip.Setup(x => x.Size).Returns(2);
            _gameSettings.ShipSizes = new List<int> { 2 };
            _gameSettings.BoardSizeX = 3;
            _gameSettings.BoardSizeY = 3;
        }

        [Test]
        [TestCaseSource(nameof(ShipsCoordinates))]
        public void Board_PopulatesPlayerFields(IList<IField> fields)
        {
            _mockShip.Setup(x => x.Coordinates).Returns(fields);
            board = new Board(new IShip[] { _mockShip.Object }, _shipsGroupValidator, _gameSettings);

            Assert.IsNotNull(board.Fields);
            Assert.IsNotEmpty(board.Fields);
        }

        [Test]
        public void Board_PopulatesComputerFields()
        {
            board = new Board(_shipPositionsRandomizer, _gameSettings);

            Assert.IsNotNull(board.Fields);
            Assert.IsNotEmpty(board.Fields);
        }

        [Test]
        [TestCaseSource(nameof(ShipsCoordinates))]
        public void AreAllShipsSunk_AllShipsSunk_ReturnsTrue(IList<IField> fields)
        {
            _mockShip.Setup(x => x.Coordinates).Returns(fields);
            _mockShip.Setup(x => x.TryToShoot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            _mockShip.Setup(x => x.IsSunk).Returns(true);
            board = new Board(new IShip[] { _mockShip.Object }, _shipsGroupValidator, _gameSettings);

            board.Shoot(1, 1);
            board.Shoot(1, 2);
            board.Shoot(2, 1);
            board.Shoot(2, 2);

            Assert.IsTrue(board.AreAllShipsSunk);
        }

        [Test]
        [TestCaseSource(nameof(ShipsCoordinates))]
        public void AreAllShipsSunk_NoShipsSunk_ReturnsFalse(IList<IField> fields)
        {
            _mockShip.Setup(x => x.Coordinates).Returns(fields);
            _mockShip.Setup(x => x.TryToShoot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            _mockShip.Setup(x => x.IsSunk).Returns(false);
            board = new Board(new IShip[] { _mockShip.Object }, _shipsGroupValidator, _gameSettings);

            board.Shoot(1, 1);

            Assert.IsFalse(board.AreAllShipsSunk);
        }    

        [Test]
        [TestCaseSource(nameof(ShipsCoordinates))]
        public void Shoot_AtShipField_ReturnsTrue(IList<IField> fields)
        {
            _mockShip.Setup(x => x.Coordinates).Returns(fields);
            _mockShip.Setup(x => x.TryToShoot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            board = new Board(new IShip[] { _mockShip.Object }, _shipsGroupValidator, _gameSettings);
            var positionX = 1;
            var positionY = 1;

            var result = board.Shoot(positionX, positionY);

            Assert.IsTrue(result.IsShipHit);
        }

        [Test]
        [TestCaseSource(nameof(ShipsCoordinates))]
        public void Shoot_AtEmptyField_ReturnsFalse(IList<IField> fields)
        {
            _mockShip.Setup(x => x.Coordinates).Returns(fields);
            _mockShip.Setup(x => x.TryToShoot(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            board = new Board(new IShip[] { _mockShip.Object }, _shipsGroupValidator, _gameSettings);
            var positionX = 0;
            var positionY = 0;

            var result = board.Shoot(positionX, positionY);

            Assert.IsFalse(result.IsShipHit);
        }
    }
}
