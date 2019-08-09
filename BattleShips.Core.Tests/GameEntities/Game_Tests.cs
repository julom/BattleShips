using System.Collections.Generic;
using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Enums;
using BattleShips.Core.GameEntities.Factories.Abstract;
using Moq;
using NUnit.Framework;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class Game_Tests
    {
        Game game;
        Mock<IBoardFactory> mockBoardFactory;
        Mock<IBoard> mockBoardPlayer;
        Mock<IBoard> mockBoardComputer;
        Mock<IDifficultyLevel> mockDifficultyLevel;
        Mock<IShip> mockShip;

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
        public void Init()
        {
            mockBoardFactory = new Mock<IBoardFactory>();
            mockBoardPlayer = new Mock<IBoard>();
            mockBoardComputer = new Mock<IBoard>();
            mockDifficultyLevel = new Mock<IDifficultyLevel>();
            mockShip = new Mock<IShip>();

            mockShip.Setup(x => x.Size).Returns(2);
            mockBoardFactory.Setup(x => x.CreateBoard()).Returns(mockBoardComputer.Object);
            mockBoardFactory.Setup(x => x.CreateBoard(It.IsNotNull<IShip[]>())).Returns(mockBoardPlayer.Object);

            mockBoardPlayer.Name = nameof(mockBoardPlayer);
            mockBoardComputer.Name = nameof(mockBoardComputer);
        }

        [Test]
        public void Game_CreatesNewGuid()
        {
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.IsNotNull(game.Guid, "Game should create new not empty Guid");
            Assert.Greater(game.Guid, System.Guid.Empty, "Game should create new not empty Guid");
        }

        [Test]
        [TestCaseSource(nameof(ShipsCoordinates))]
        public void Game_CreatesComputerAndPlayerBoards(IList<IField> fields)
        {
            mockShip.Setup(x => x.Coordinates).Returns(fields);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.AreSame(mockBoardComputer.Object, game.ComputerBoard);
            Assert.AreSame(mockBoardPlayer.Object, game.PlayerBoard);
        }

        [Test]
        public void Game_IsWon_ComputerShipsAreSunk()
        {
            mockBoardComputer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.IsTrue(game.IsWon);
        }

        [Test]
        public void Game_IsLost_PlayerShipsAreSunk()
        {
            mockBoardPlayer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.IsTrue(game.IsLost);
        }

        [Test]
        public void MakeComputerMovement_UsesDifficultyLevelToChooseShotCoordinates()
        {
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, mockDifficultyLevel.Object);

            game.MakeComputerMovement();

            mockDifficultyLevel.Verify(x => x.ChooseShotCoordinates(mockBoardPlayer.Object), Times.Once,
                "Computer movement should rely on IDifficultyLevel interface");
        }

        [Test]
        public void MakeComputerMovement_ReturnsExpectedShootResult([Values] bool isHit, [Values] bool isSunk)
        {
            var expectedResult = new ShootResultDTO() {IsShipHit = isHit, IsShipSunk = isSunk };
            mockBoardPlayer.Setup(x => x.Shoot(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, mockDifficultyLevel.Object);

            var result = game.MakeComputerMovement();

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void MakePlayerMovement_ReturnsExpectedShootResult([Values] bool isHit, [Values] bool isSunk)
        {
            var expectedResult = new ShootResultDTO() { IsShipHit = isHit, IsShipSunk = isSunk };
            mockBoardComputer.Setup(x => x.Shoot(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, mockDifficultyLevel.Object);

            var result = game.MakePlayerMovement(0, 0);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void MakePlayerMovement_AfterGameIsLost_ThrowsException()
        {
            mockBoardPlayer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, mockDifficultyLevel.Object);

            Assert.Throws<GameLogicalException>(() => game.MakePlayerMovement(0, 0));
        }

        [Test]
        public void MakePlayerMovement_AfterGameIsWon_ThrowsException()
        {
            mockBoardComputer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, mockDifficultyLevel.Object);

            Assert.Throws<GameLogicalException>(() => game.MakePlayerMovement(0, 0));
        }

        [Test]
        public void MakeComputerMovement_AfterGameIsLost_ThrowsException()
        {
            mockBoardPlayer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, mockDifficultyLevel.Object);

            Assert.Throws<GameLogicalException>(() => game.MakeComputerMovement());
        }

        [Test]
        public void MakeComputerMovement_AfterGameIsWon_ThrowsException()
        {
            mockBoardComputer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new IShip[] { mockShip.Object }, mockBoardFactory.Object, mockDifficultyLevel.Object);

            Assert.Throws<GameLogicalException>(() => game.MakeComputerMovement());
        }
    }
}
