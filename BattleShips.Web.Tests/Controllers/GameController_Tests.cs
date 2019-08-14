using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.Web.Controllers;
using BattleShips.Web.Models;
using BattleShips.Web.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Web.Tests.Controllers
{
    [TestFixture]
    public class GameController_Tests
    {
        private Mock<IServiceProvider> mockServiceProvider;
        private Mock<ILogger<GameController>> mockLogger;
        private Mock<IGameService> mockGameService;

        [SetUp]
        public void SetUp()
        {
            mockServiceProvider = new Mock<IServiceProvider>();
            mockLogger = new Mock<ILogger<GameController>>();
            mockGameService = new Mock<IGameService>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IGameService))).Returns(mockGameService.Object);
        }

        private GameController CreateGameController()
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            return new GameController(mockServiceProvider.Object, mockLogger.Object)
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
        public void CheckShips_GameServiceThrowsException_MessageToUserIsSavedToTempData()
        {
            var gameController = CreateGameController();
            var exception = new Exception("TestException");
            mockGameService.Setup(x => x.GetShipsPositions(It.IsAny<IList<ShipLayout>>())).Throws(exception);
            UserShipsLocationViewModel userShipsLocationVM = new UserShipsLocationViewModel();
            
            gameController.CheckShips(userShipsLocationVM);

            var messageToUser = gameController.TempData[nameof(UserCommunicationViewModel.MessageToUser)] as string;
            Assert.IsNotNull(messageToUser, "Message to user should be saved in TempData");
            Assert.That(messageToUser.Contains(exception.Message), "Message to user should contain exception message");
        }

        [Test]
        public void CheckShips_GameServiceThrowsException_UserShipLocationIsSavedToTempData()
        {
            var gameController = CreateGameController();
            mockGameService.Setup(x => x.GetShipsPositions(It.IsAny<IList<ShipLayout>>())).Throws<Exception>();
            UserShipsLocationViewModel userShipsLocationVM = new UserShipsLocationViewModel() { ShipsFields = new[] { false, true } };

            gameController.CheckShips(userShipsLocationVM);


            var serializedShipsLocation = gameController.TempData[nameof(UserShipsLocationViewModel)] as string;
            Assert.IsNotNull(serializedShipsLocation, "Player ships location should be saved in TempData");

            var deserializedUserShipsLocationVM = JsonConvert.DeserializeObject<UserShipsLocationViewModel>(serializedShipsLocation);
            Assert.AreEqual(userShipsLocationVM.ShipsFields, deserializedUserShipsLocationVM.ShipsFields, "Ships locations should be the same");
        }

        [Test]
        public void CheckShips_GameServiceThrowsException_ReturnsRedirectToActionResult()
        {
            var gameController = CreateGameController();
            mockGameService.Setup(x => x.GetShipsPositions(It.IsAny<IList<ShipLayout>>())).Throws<Exception>();
            UserShipsLocationViewModel userShipsLocationVM = null;

            var result = gameController.CheckShips(userShipsLocationVM);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Test]
        public void CheckShips_ServiceProviderThrowsException_ExceptionDoesNotPropagate()
        {
            mockServiceProvider.Setup(x => x.GetService(It.IsAny<Type>())).Throws<Exception>();
            var gameController = CreateGameController();
            UserShipsLocationViewModel userShipsLocationVM = null;

            Action action = () => gameController.CheckShips(userShipsLocationVM);

            Assert.DoesNotThrow(action.Invoke);
        }

        [Test]
        public void CheckShips_HappyPath_UserShipLocationIsSavedToTempData()
        {
            var gameController = CreateGameController();
            var shipFields = new[] { false, true };
            mockGameService.Setup(x => x.GetShipsPositions(It.IsAny<IList<ShipLayout>>())).Returns(shipFields);
            UserShipsLocationViewModel userShipsLocationVM = new UserShipsLocationViewModel();

            gameController.CheckShips(userShipsLocationVM);


            var serializedShipsLocation = gameController.TempData[nameof(UserShipsLocationViewModel)] as string;
            Assert.IsNotNull(serializedShipsLocation, "Player ships location should be saved in TempData");

            var deserializedUserShipsLocationVM = JsonConvert.DeserializeObject<UserShipsLocationViewModel>(serializedShipsLocation);
            Assert.AreEqual(shipFields, deserializedUserShipsLocationVM.ShipsFields, "Ships locations should be the same");
        }

        [Test]
        public void CheckShips_HappyPath_ReturnsRedirectToActionResult()
        {
            var gameController = CreateGameController();
            mockGameService.Setup(x => x.GetShipsPositions(It.IsAny<IList<ShipLayout>>())).Returns(It.IsAny<bool[]>());
            UserShipsLocationViewModel userShipsLocationVM = null;

            var result = gameController.CheckShips(userShipsLocationVM);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Test]
        public void Create_ModelIsNotValid_ReturnsViewResult()
        {
            var gameController = CreateGameController();
            gameController.ModelState.AddModelError("errorKey", "errorMessage");
            GameModel gameModel = null;
            UserShipsLocationViewModel userShipsLocationVM = null;

            var result = gameController.Create(gameModel, userShipsLocationVM);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void Create_ModelIsNotValid_AddsMessageToTempData()
        {
            var gameController = CreateGameController();
            gameController.ModelState.AddModelError("errorKey", "errorMessage");
            GameModel gameModel = null;
            UserShipsLocationViewModel userShipsLocationVM = null;

            var result = gameController.Create(gameModel, userShipsLocationVM);

            Assert.IsAssignableFrom<ViewResult>(result);

            var messageToUser = gameController.TempData[nameof(UserCommunicationViewModel.MessageToUser)] as string;
            Assert.IsNotNull(messageToUser, "Message to user should be saved in TempData");
        }

        [Test]
        public void Create_HappyPath_ReturnsViewResult()
        {
            var gameController = CreateGameController();
            GameModel gameModel = new GameModel();
            UserShipsLocationViewModel userShipsLocationVM = new UserShipsLocationViewModel();

            var result = gameController.Create(gameModel, userShipsLocationVM);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void Create_GameServiceThrowsException_GameServiceRemovesGame()
        {
            var gameController = CreateGameController();
            mockGameService.Setup(x => x.InitializeGame(It.IsAny<IList<ShipLayout>>(), It.IsAny<IDifficultyLevel>())).Throws<Exception>();
            GameModel gameModel = new GameModel();
            UserShipsLocationViewModel userShipsLocationVM = new UserShipsLocationViewModel();

            var result = gameController.Create(gameModel, userShipsLocationVM);

            mockGameService.Verify(x => x.RemoveGame(gameModel.GameGuid), Times.Once);
        }

        [Test]
        public void Create_GameServiceThrowsException_ShipsLocationIsSavedToTempData()
        {
            var gameController = CreateGameController();
            var shipFields = new[] { false, true };
            mockGameService.Setup(x => x.InitializeGame(It.IsAny<IList<ShipLayout>>(), It.IsAny<IDifficultyLevel>())).Throws<Exception>();
            GameModel gameModel = new GameModel();
            UserShipsLocationViewModel userShipsLocationVM = new UserShipsLocationViewModel() { ShipsFields = shipFields };

            var result = gameController.Create(gameModel, userShipsLocationVM);

            var serializedShipsLocation = gameController.TempData[nameof(UserShipsLocationViewModel)] as string;
            Assert.IsNotNull(serializedShipsLocation, "Player ships location should be saved in TempData");

            var deserializedUserShipsLocationVM = JsonConvert.DeserializeObject<UserShipsLocationViewModel>(serializedShipsLocation);
            Assert.AreEqual(shipFields, deserializedUserShipsLocationVM.ShipsFields, "Ships locations should be the same");
        }

        [Test]
        public void Create_GameServiceThrowsException_MessageToUserIsSavedToTempData()
        {
            var gameController = CreateGameController();
            mockGameService.Setup(x => x.InitializeGame(It.IsAny<IList<ShipLayout>>(), It.IsAny<IDifficultyLevel>())).Throws<Exception>();
            GameModel gameModel = new GameModel();
            UserShipsLocationViewModel userShipsLocationVM = new UserShipsLocationViewModel();

            var result = gameController.Create(gameModel, userShipsLocationVM);

            var messageToUser = gameController.TempData[nameof(UserCommunicationViewModel.MessageToUser)] as string;
            Assert.IsNotNull(messageToUser, "Message to user should be saved in TempData");
        }

        [Test]
        public void Create_GameServiceThrowsException_ReturnsRedirectToActionResult()
        {
            mockServiceProvider.Setup(x => x.GetService(It.IsAny<Type>())).Throws<Exception>();
            var gameController = CreateGameController();
            GameModel gameModel = null;
            UserShipsLocationViewModel userShipsLocationVM = null;

            var result = gameController.Create(gameModel, userShipsLocationVM);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Test]
        public void Create_ServiceProviderThrowsException_ExceptionDoesNotPropagate()
        {
            mockServiceProvider.Setup(x => x.GetService(It.IsAny<Type>())).Throws<Exception>();
            var gameController = CreateGameController();
            GameModel gameModel = null;
            UserShipsLocationViewModel userShipsLocationVM = null;

            Action action = () => gameController.Create(gameModel, userShipsLocationVM);

            Assert.DoesNotThrow(action.Invoke);
        }

        [Test]
        public void NextTurn_InvalidModelState_RedirectToActionResult()
        {
            var gameController = CreateGameController();
            gameController.ModelState.AddModelError("errorKey", "errorMessage");
            int? shootPositionX = null;
            int? shootPositionY = null;
            GameModel gameModel = new GameModel();

            var result = gameController.NextTurn(shootPositionX, shootPositionY, gameModel);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Test]
        public void NextTurn_InvalidModelState_MessageToUserSavedToTempData()
        {
            var gameController = CreateGameController();
            gameController.ModelState.AddModelError("errorKey", "errorMessage");
            int? shootPositionX = null;
            int? shootPositionY = null;
            GameModel gameModel = new GameModel();

            gameController.NextTurn(shootPositionX, shootPositionY, gameModel);

            var messageToUser = gameController.TempData[nameof(UserCommunicationViewModel.MessageToUser)] as string;
            Assert.IsNotNull(messageToUser, "Message to user should be saved in TempData");
        }

        [Test]
        public void NextTurn_GameServiceThrowsException_MessageToUserIsSavedToTempData()
        {
            var gameController = CreateGameController();
            mockGameService.Setup(x => x.TakeNextRound(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>())).Throws<Exception>();
            int? shootPositionX = null;
            int? shootPositionY = null;
            GameModel gameModel = new GameModel();

            gameController.NextTurn(shootPositionX, shootPositionY, gameModel);

            var messageToUser = gameController.TempData[nameof(UserCommunicationViewModel.MessageToUser)] as string;
            Assert.IsNotNull(messageToUser, "Message to user should be saved in TempData");
        }

        [Test]
        public void NextTurn_GameServiceThrowsException_ReturnsRedirectToActionResult()
        {
            var gameController = CreateGameController();
            mockGameService.Setup(x => x.TakeNextRound(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>())).Throws<Exception>();

            var result = gameController.NextTurn(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GameModel>());

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Test]
        public void NextTurn_ServiceProviderThrowsException_ExceptionDoesNotPropagate()
        {
            mockServiceProvider.Setup(x => x.GetService(It.IsAny<Type>())).Throws<Exception>();
            var gameController = CreateGameController();

            Action action = () => gameController.NextTurn(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GameModel>());

            Assert.DoesNotThrow(action.Invoke);
        }

        [Test]
        public void NextTurn_HappyPath_ReturnsViewResult()
        {
            var gameController = CreateGameController();
            mockGameService.Setup(x => x.TakeNextRound(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>())).Returns(It.IsAny<IList<string>>());
            int? shootPositionX = 0;
            int? shootPositionY = 0;
            GameModel gameModel = new GameModel() { GameGuid = Guid.Empty };

            var result = gameController.NextTurn(shootPositionX, shootPositionY, gameModel);

            Assert.IsAssignableFrom<ViewResult>(result);
        }
    }
}
