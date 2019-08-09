using BattleShips.Core;
using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Enums;
using BattleShips.Core.GameEntities.Factories;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.Core.GameEntities.Utils.Abstract;
using BattleShips.GameRepository;
using BattleShips.Tests;
using BattleShips.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Web.Tests.Services
{
    [TestFixture]
    public class GameServiceTests
    {

        IGameSettings _gameSettings;
        private Mock<IGameRepository> mockGameRepository;
        private Mock<IGame> mockGame;
        private Mock<IShipFactory> mockShipFactory;
        private Mock<IShip> mockShip;
        private Mock<IDifficultyLevel> mockDifficultyLevel;
        private Mock<IGameStatusUpdater> mockGameStatusUpdater;

        public GameServiceTests()
        {
            IServiceProvider serviceProvider = DIContainersTestConfiguration.GetDIServiceProvider();
            _gameSettings = serviceProvider.GetService<IGameSettings>();
        }

        [SetUp]
        public void SetUp()
        {
            mockGame = new Mock<IGame>();
            mockGameRepository = new Mock<IGameRepository>();
            mockGameRepository.Setup(x => x.CreateGame(It.IsAny<IShip[]>(), It.IsAny<IDifficultyLevel>())).Returns(mockGame.Object);
            mockShipFactory = new Mock<IShipFactory>();
            mockShip = new Mock<IShip>();
            mockShipFactory.Setup(x => x.Create(It.IsAny<ShipVector>(), It.IsAny<ShipVector>())).Returns(mockShip.Object);
            mockDifficultyLevel = new Mock<IDifficultyLevel>();
            mockGameStatusUpdater = new Mock<IGameStatusUpdater>();
        }

        private GameService CreateService()
        {
            return new GameService(
                _gameSettings,
                mockGameRepository.Object,
                mockShipFactory.Object,
                mockGameStatusUpdater.Object);
        }

        [Test]
        public void GetShipsPositions_ShipHasTwoFields_ResultHasTableWithTwoTrues()
        {
            var service = CreateService();
            var shipsFields = new List<IField>() { new Field(FieldTypes.Ship, 0, 1), new Field(FieldTypes.Ship, 0, 2) };
            var shipLayouts = new List<ShipLayout> { new ShipLayout(0, 0, 1, 2) };
            mockShip.Setup(x => x.Coordinates).Returns(shipsFields);

            var result = service.GetShipsPositions(shipLayouts);

            Assert.AreEqual(shipsFields.Count(), result.Count(field => field));
        }

        [Test]
        public void GetShipsPositions_ShipHasTwoFields_ResultHasTableWithTruesOnTheSamePositions()
        {
            var service = CreateService();
            var shipsFields = new List<IField>() { new Field(FieldTypes.Ship, 0, 1), new Field(FieldTypes.Ship, 0, 2) };
            var shipLayouts = new List<ShipLayout> { new ShipLayout(0, 0, 1, 2) };
            mockShip.Setup(x => x.Coordinates).Returns(shipsFields);

            var result = service.GetShipsPositions(shipLayouts);

            Assert.IsTrue(result[shipsFields[0].PositionY * _gameSettings.BoardSizeY + shipsFields[0].PositionX]);
            Assert.IsTrue(result[shipsFields[1].PositionY * _gameSettings.BoardSizeY + shipsFields[1].PositionX]);
        }

        [Test]
        public void InitializeGame_ReturnsGameCreatedByGameFactory()
        {
            var service = CreateService();
            var shipLayouts = new List<ShipLayout> { It.IsAny<ShipLayout>() };

            var result = service.InitializeGame(shipLayouts, mockDifficultyLevel.Object);

            Assert.AreSame(mockGame.Object, result);
        }

        [Test]
        public void RemoveGame_GameExistsInRepositoryAndGuidIsNotNull_ReturnsTrue()
        {
            var service = CreateService();
            mockGameRepository.Setup(x => x.DeleteGame(It.IsAny<Guid>())).Returns(true);

            var result = service.RemoveGame(It.IsNotNull<Guid>());

            Assert.IsTrue(result);
        }

        [Test]
        public void RemoveGame_GameNotExistInRepositoryAndGuidIsNotNull_ReturnsFalse()
        {
            var service = CreateService();
            mockGameRepository.Setup(x => x.DeleteGame(It.IsAny<Guid>())).Returns(false);

            var result = service.RemoveGame(It.IsNotNull<Guid>());

            Assert.IsFalse(result);
        }

        [Test]
        public void RemoveGame_GameExistsInRepositoryAndGuidIsNull_ReturnsFalse()
        {
            var service = CreateService();
            mockGameRepository.Setup(x => x.DeleteGame(It.IsAny<Guid>())).Returns(true);

            var result = service.RemoveGame(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void TakeNextRound_GameRepositoryDoesNoTContainGame_ThrowsException()
        {
            var service = CreateService();
            mockGameRepository.Setup(x => x.GetGame(It.IsAny<Guid>())).Returns((IGame)null);

            Action action = () => service.TakeNextRound(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>());

            Assert.Throws<GameException>(() => action());
        }

        [Test]
        public void TakeNextRound_GameIsNotWon_GameStatusUpdaterRunsTwoTimes()
        {
            var service = CreateService();
            mockGame.Setup(x => x.IsWon).Returns(false);
            mockGameRepository.Setup(x => x.GetGame(It.IsAny<Guid>())).Returns(mockGame.Object);
            mockGameRepository.Setup(x => x.UpdateGame(It.IsAny<Guid>(), mockGame.Object));
            mockGameStatusUpdater.Setup(x => x.UpdateGameStatus(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<ShootResultDTO>(), mockGame.Object));

            service.TakeNextRound(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>());

            mockGameStatusUpdater.Verify(x => x.UpdateGameStatus(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<ShootResultDTO>(), mockGame.Object), Times.Exactly(2),
                "When game is not won, status should be updated for player and for computer opponent");
        }

        [Test]
        public void TakeNextRound_GameIsWon_GameStatusUpdaterRunsOnce()
        {
            var service = CreateService();
            mockGame.Setup(x => x.IsWon).Returns(true);
            mockGameRepository.Setup(x => x.GetGame(It.IsAny<Guid>())).Returns(mockGame.Object);
            mockGameRepository.Setup(x => x.UpdateGame(It.IsAny<Guid>(), mockGame.Object));
            mockGameStatusUpdater.Setup(x => x.UpdateGameStatus(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<ShootResultDTO>(), mockGame.Object));

            service.TakeNextRound(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>());

            mockGameStatusUpdater.Verify(x => x.UpdateGameStatus(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<ShootResultDTO>(), mockGame.Object), Times.Once,
                "When game is won, status should be updated only for player");
        }

        [Test]
        public void TakeNextRound_RepositioryContainGame_PlayerMovementTaken()
        {
            var service = CreateService();
            mockGame.Setup(x => x.IsWon).Returns(It.IsAny<bool>());
            mockGameRepository.Setup(x => x.GetGame(It.IsAny<Guid>())).Returns(mockGame.Object);
            mockGameRepository.Setup(x => x.UpdateGame(It.IsAny<Guid>(), mockGame.Object));
            mockGameStatusUpdater.Setup(x => x.UpdateGameStatus(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<ShootResultDTO>(), mockGame.Object));

            service.TakeNextRound(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>());

            mockGame.Verify(x => x.MakePlayerMovement(It.IsAny<int>(), It.IsAny<int>()), Times.Once,
                "When game is found in repository, game should run PlayerMovment");
        }

        [Test]
        public void TakeNextRound_GameIsWon_ComputerMovementNotTaken()
        {
            var service = CreateService();
            mockGame.Setup(x => x.IsWon).Returns(true);
            mockGameRepository.Setup(x => x.GetGame(It.IsAny<Guid>())).Returns(mockGame.Object);
            mockGameRepository.Setup(x => x.UpdateGame(It.IsAny<Guid>(), mockGame.Object));
            mockGameStatusUpdater.Setup(x => x.UpdateGameStatus(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<ShootResultDTO>(), mockGame.Object));

            service.TakeNextRound(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>());

            mockGame.Verify(x => x.MakeComputerMovement(), Times.Never,
                "When game is won, game shoul not run ComputerMovement");
        }
    }
}
