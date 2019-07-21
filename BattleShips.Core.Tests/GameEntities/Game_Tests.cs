using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels;
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
        Mock<IBoardFactory> mockBoardFactory = new Mock<IBoardFactory>();
        Mock<IBoard> mockBoardPlayer = new Mock<IBoard>();
        Mock<IBoard> mockBoardComputer = new Mock<IBoard>();

        [SetUp]
        public void Init()
        {
            mockBoardFactory.Setup(x => x.CreateBoard(It.IsAny<bool[,]>())).Returns(mockBoardComputer.Object);
            mockBoardFactory.Setup(x => x.CreateBoard(It.IsNotNull<bool[,]>())).Returns(mockBoardPlayer.Object);

        }

        [Test]
        public void Game_HasTwoNotNullBoards()
        {
            game = new Game(mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.IsNotNull(game.ComputerBoard);
            Assert.IsNotNull(game.PlayerBoard);
        }

        [Test]
        public void Game_IsWon_ComputerShipsAreSunk()
        {
            mockBoardComputer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.IsTrue(game.IsWon);
        }

        [Test]
        public void Game_IsLost_PlayerShipsAreSunk()
        {
            mockBoardPlayer.Setup(x => x.AreAllShipsSunk).Returns(true);
            game = new Game(mockBoardFactory.Object, new DifficultyLevelEasy());

            Assert.IsTrue(game.IsLost);
        }

        [Test]
        public void MakeComputerMovement_ReturnsArrayWithShot()
        {
            var fields = new Field[,] { { new Field(FieldTypes.Empty, 0, 0) } };
            mockBoardComputer.Setup(x => x.Fields).Returns(fields);
            mockBoardPlayer.Setup(x => x.Fields).Returns(fields);
            game = new Game(mockBoardFactory.Object, new DifficultyLevelEasy());

            game.MakeComputerMovement();

            Assert.AreNotEqual(fields, game.PlayerBoard);
        }

        [Test]
        public void MakePlayerMovement_ReturnsArrayWithShot()
        {
            var fields = new Field[,] { { new Field(FieldTypes.Empty, 0, 0) } };
            mockBoardComputer.Setup(x => x.Fields).Returns(fields);
            mockBoardPlayer.Setup(x => x.Fields).Returns(fields);
            game = new Game(mockBoardFactory.Object, new DifficultyLevelEasy());

            game.MakePlayerMovement(0, 0);

            Assert.AreNotEqual(fields, game.PlayerBoard);
        }
    }
}
