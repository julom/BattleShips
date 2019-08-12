using BattleShips.Web.Controllers;
using BattleShips.Web.Models;
using BattleShips.Web.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;

namespace BattleShips.Web.Tests.Controllers
{
    [TestFixture]
    public class GameControllerTests
    {
        private Mock<IServiceProvider> mockServiceProvider;
        private Mock<IGameService> gameService;

        [SetUp]
        public void SetUp()
        {
            mockServiceProvider = new Mock<IServiceProvider>();
            gameService = new Mock<IGameService>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IGameService))).Returns(gameService.Object);

        }

        private GameController CreateGameController()
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            return new GameController(mockServiceProvider.Object)
            {
                TempData = tempData
            };
        }

        [Test]
        public void Index_TempDataContainsMessage_AddMessageToModel()
        {
            var gameController = CreateGameController();
            GameModel gameModel = new GameModel();
            string testedMsg = "Test message";
            gameController.TempData[nameof(UserCommunicationViewModel.MessageToUser)] = testedMsg;

            gameController.Index(gameModel);

            Assert.AreEqual(testedMsg, gameModel.UserCommunicationVM.MessageToUser.SingleOrDefault());
        }

        [Test]
        public void Index_TempDataContainsShipsFields_AddShipFieldsToModel()
        {
            var gameController = CreateGameController();
            GameModel gameModel = new GameModel();
            bool[] shipsFields = new[] { false, true };
            var userShipsLocationVM = new UserShipsLocationViewModel() { ShipsFields = shipsFields };
            var serializedVm = JsonConvert.SerializeObject(userShipsLocationVM);
            gameController.TempData[nameof(UserShipsLocationViewModel)] = serializedVm;

            gameController.Index(gameModel);

            Assert.AreEqual(shipsFields, gameModel.ShipsFields);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            var gameController = CreateGameController();
            GameModel gameModel = new GameModel();

            var result = gameController.Index(gameModel);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void CheckShips_StateUnderTest_ExpectedBehavior()
        {
            var gameController = CreateGameController();
            UserShipsLocationViewModel userShipsLocationVM = null;

            var result = gameController.CheckShips(userShipsLocationVM);

            Assert.Fail();
        }

        [Test]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            var gameController = this.CreateGameController();
            GameModel gameModel = null;
            UserShipsLocationViewModel userShipsLocationVM = null;

            var result = gameController.Create(
                gameModel,
                userShipsLocationVM);

            Assert.Fail();
        }

        [Test]
        public void NextTurn_StateUnderTest_ExpectedBehavior()
        {
            var gameController = this.CreateGameController();
            int? shootPositionX = null;
            int? shootPositionY = null;
            GameModel gameModel = null;

            var result = gameController.NextTurn(
                shootPositionX,
                shootPositionY,
                gameModel);

            Assert.Fail();
        }
    }
}
