using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Utils;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.Tests.GameEntities.Utils
{
    [TestFixture]
    public class GameStatusUpdater_Tests
    {
        Mock<IGame> mockGame;

        [SetUp]
        public void SetUp()
        {
            mockGame = new Mock<IGame>();
        }

        private GameStatusUpdater CreateGameStatusUpdater()
        {
            return new GameStatusUpdater();
        }

        [Test]
        public void UpdateGameStatus_ShootResultIsNull_ThrowsException()
        {
            var gameStatusUpdater = CreateGameStatusUpdater();
            var gameStatusList = new List<string>();
            string person = "";
            ShootResultDTO shootResult = null;

            Assert.Throws<GameArgumentException>(() => gameStatusUpdater.UpdateGameStatus(
                gameStatusList,
                person,
                shootResult,
                mockGame.Object));
        }

        [Test]
        public void UpdateGameStatus_GameIsNull_ThrowsException()
        {
            var gameStatusUpdater = CreateGameStatusUpdater();
            var gameStatusList = new List<string>();
            string person = "";
            ShootResultDTO shootResult = new ShootResultDTO();
            IGame game = null;

            Assert.Throws<GameArgumentException>(() => gameStatusUpdater.UpdateGameStatus(
                gameStatusList,
                person,
                shootResult,
                game));
        }

        [Test]
        public void UpdateGameStatus_ShipIsNotSunk_GameStatusListContainsTwoElements()
        {
            var gameStatusUpdater = CreateGameStatusUpdater();
            var gameStatusList = new List<string>();
            string person = "";
            ShootResultDTO shootResult = new ShootResultDTO() { PositionX = It.IsAny<int>(), PositionY = It.IsAny<int>(), IsShipSunk = false };

            gameStatusUpdater.UpdateGameStatus(
                gameStatusList,
                person,
                shootResult,
                mockGame.Object);

            Assert.AreEqual(2, gameStatusList.Count);
        }

        [Test]
        public void UpdateGameStatus_ShipIsSunk_GameStatusListContainsThreeElements()
        {
            var gameStatusUpdater = CreateGameStatusUpdater();
            var gameStatusList = new List<string>();
            string person = "";
            ShootResultDTO shootResult = new ShootResultDTO() { PositionX = It.IsAny<int>(), PositionY = It.IsAny<int>(), IsShipSunk = true };

            gameStatusUpdater.UpdateGameStatus(
                gameStatusList,
                person,
                shootResult,
                mockGame.Object);

            Assert.AreEqual(3, gameStatusList.Count);
        }

        [Test]
        public void UpdateGameStatus_ShipIsSunkGameIsWon_GameStatusListContainsFourElements()
        {
            var gameStatusUpdater = CreateGameStatusUpdater();
            var gameStatusList = new List<string>();
            string person = "";
            ShootResultDTO shootResult = new ShootResultDTO() { PositionX = It.IsAny<int>(), PositionY = It.IsAny<int>(), IsShipSunk = true };
            mockGame.Setup(x => x.IsWon).Returns(true);

            gameStatusUpdater.UpdateGameStatus(
                gameStatusList,
                person,
                shootResult,
                mockGame.Object);

            Assert.AreEqual(4, gameStatusList.Count);
        }

        [Test]
        public void UpdateGameStatus_ShipIsSunkGameIsLost_GameStatusListContainsFourElements()
        {
            var gameStatusUpdater = CreateGameStatusUpdater();
            var gameStatusList = new List<string>();
            string person = "";
            ShootResultDTO shootResult = new ShootResultDTO() { PositionX = It.IsAny<int>(), PositionY = It.IsAny<int>(), IsShipSunk = true };
            mockGame.Setup(x => x.IsLost).Returns(true);

            gameStatusUpdater.UpdateGameStatus(
                gameStatusList,
                person,
                shootResult,
                mockGame.Object);

            Assert.AreEqual(4, gameStatusList.Count);
        }
    }
}
