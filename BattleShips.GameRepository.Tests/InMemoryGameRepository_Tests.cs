using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;
using BattleShips.GameRepository;
using BattleShips.Tests;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;

namespace Tests
{
    public class InMemoryGameRepository_Tests
    {
        private readonly Guid guidGame = Guid.NewGuid();
        private readonly Guid guidNotGame = Guid.NewGuid();
        private readonly IDifficultyLevel _difficulty;
        InMemoryGameRepository gameRepository;
        Mock<IGameFactory> mockGameFactory;
        Mock<IGame> mockGame;

        public InMemoryGameRepository_Tests()
        {
            IServiceProvider serviceProvider = DIContainersTestConfiguration.GetDIServiceProvider();
            _difficulty = serviceProvider.GetService<IDifficultyLevel>();
        }

        private IGame CreateGame()
        {
            return gameRepository.CreateGame(It.IsAny<IShip[]>(), _difficulty);
        }

        [SetUp]
        public void Init()
        {
            mockGameFactory = new Mock<IGameFactory>();
            mockGame = new Mock<IGame>();
            mockGame.Name = "Game1";
            mockGame.Setup(x => x.Guid).Returns(guidGame);
            mockGameFactory.Setup(x => x.Create(It.IsAny<IShip[]>(), It.IsAny<IDifficultyLevel>())).Returns(mockGame.Object);
            gameRepository = new InMemoryGameRepository(mockGameFactory.Object);
        }

        [Test]
        public void CreateGame_ReturnsNewGame()
        {
            var result = CreateGame();

            Assert.AreSame(mockGame.Object, result);
        }

        [Test]
        public void GetGame_GameExists_ReturnsSaved()
        {
            CreateGame();
            var result = gameRepository.GetGame(guidGame);

            Assert.AreSame(mockGame.Object, result);
        }

        [Test]
        public void GetGame_GameNotExist_ReturnsNull()
        {
            CreateGame();
            var result = gameRepository.GetGame(guidNotGame);

            Assert.IsNull(result);
        }

        [Test]
        public void UpdateGame_GameExists_SavesGame()
        {
            CreateGame();
            var mockGame2 = new Mock<IGame>();
            mockGame2.Setup(x => x.Guid).Returns(guidGame);
            mockGame2.Name = "Game2";

            gameRepository.UpdateGame(guidGame, mockGame2.Object);
            var result = gameRepository.GetGame(guidGame);

            Assert.AreEqual(mockGame2.Name, Mock.Get(result).Name);
        }

        [Test]
        public void UpdateGame_GameNotExist_DoesNotSaveGame()
        {
            CreateGame();
            var mockGame2 = new Mock<IGame>();
            mockGame2.Setup(x => x.Guid).Returns(guidNotGame);
            mockGame2.Name = "Game2";

            gameRepository.UpdateGame(guidNotGame, mockGame2.Object);
            var result = gameRepository.GetGame(guidGame);

            Assert.AreNotEqual(mockGame2.Name, Mock.Get(result).Name);
        }

        [Test]
        public void DeleteGame_GameExists_DeletesGameFromMemory()
        {
            CreateGame();
            var result = gameRepository.DeleteGame(guidGame);

            Assert.IsTrue(result);
            Assert.IsNull(gameRepository.GetGame(guidGame));
        }

        [Test]
        public void DeleteGame_GameNotExist_ReturnsFalse()
        {
            var result = gameRepository.DeleteGame(guidNotGame);

            Assert.IsFalse(result);
        }
    }
}