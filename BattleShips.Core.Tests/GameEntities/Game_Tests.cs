using System.Collections.Generic;
using System.Linq;
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
        public void Init()
        {
            mockBoardFactory = new Mock<IBoardFactory>();
            mockBoardPlayer = new Mock<IBoard>();
            mockBoardComputer = new Mock<IBoard>();
            mockDifficultyLevel = new Mock<IDifficultyLevel>();

            mockBoardFactory.Setup(x => x.CreateBoard(It.IsAny<bool[,]>())).Returns(mockBoardComputer.Object);
            mockBoardFactory.Setup(x => x.CreateBoard(It.IsNotNull<bool[,]>())).Returns(mockBoardPlayer.Object);

            mockBoardPlayer.Name = nameof(mockBoardPlayer);
            mockBoardComputer.Name = nameof(mockBoardComputer);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Game_CreatesComputerAndPlayerBoards(bool[,] fields)
        {
            game = new Game(fields, mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.AreSame(mockBoardComputer.Object, game.ComputerBoard);
            Assert.AreSame(mockBoardPlayer.Object, game.PlayerBoard);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Game_IsWon_ComputerShipsAreSunk(bool[,] fields)
        {
            mockBoardComputer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(fields, mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.IsTrue(game.IsWon);
        }

        [Test]
        [TestCaseSource(nameof(FieldCoordinates))]
        public void Game_IsLost_PlayerShipsAreSunk(bool[,] fields)
        {
            mockBoardPlayer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(fields, mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.IsTrue(game.IsLost);
        }

        [Test]
        public void MakeComputerMovement_UsesDifficultyLevelToChooseShotCoordinates()
        {
            game = new Game(new bool[1, 1], mockBoardFactory.Object, mockDifficultyLevel.Object);

            game.MakeComputerMovement();

            mockDifficultyLevel.Verify(x => x.ChooseShotCoordinates(mockBoardPlayer.Object), Times.Once,
                "Computer movement should rely on IDifficultyLevel interface");
        }

        [Test]
        public void MakeComputerMovement_ReturnsExpectedShootResult([Values] bool isHit, [Values] bool isSunk)
        {
            var expectedResult = new ShootResultDTO() {IsShipHit = isHit, IsShipSunk = isSunk };
            mockBoardPlayer.Setup(x => x.Shoot(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);
            game = new Game(new bool[1,1], mockBoardFactory.Object, mockDifficultyLevel.Object);

            var result = game.MakeComputerMovement();

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void MakePlayerMovement_ReturnsExpectedShootResult([Values] bool isHit, [Values] bool isSunk)
        {
            var expectedResult = new ShootResultDTO() { IsShipHit = isHit, IsShipSunk = isSunk };
            mockBoardComputer.Setup(x => x.Shoot(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);
            game = new Game(new bool[1, 1], mockBoardFactory.Object, mockDifficultyLevel.Object);

            var result = game.MakePlayerMovement(0, 0);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void MakePlayerMovement_AfterGameIsLost_ThrowsException()
        {
            mockBoardPlayer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new bool[1, 1], mockBoardFactory.Object, mockDifficultyLevel.Object);

            Assert.Throws<GameLogicalException>(() => game.MakePlayerMovement(0, 0));
        }

        [Test]
        public void MakePlayerMovement_AfterGameIsWon_ThrowsException()
        {
            mockBoardComputer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new bool[1, 1], mockBoardFactory.Object, mockDifficultyLevel.Object);

            Assert.Throws<GameLogicalException>(() => game.MakePlayerMovement(0, 0));
        }

        [Test]
        public void MakeComputerMovement_AfterGameIsLost_ThrowsException()
        {
            mockBoardPlayer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new bool[1, 1], mockBoardFactory.Object, mockDifficultyLevel.Object);

            Assert.Throws<GameLogicalException>(() => game.MakeComputerMovement());
        }

        [Test]
        public void MakeComputerMovement_AfterGameIsWon_ThrowsException()
        {
            mockBoardComputer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(new bool[1, 1], mockBoardFactory.Object, mockDifficultyLevel.Object);

            Assert.Throws<GameLogicalException>(() => game.MakeComputerMovement());
        }
    }
}
