using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using BattleShips.Web.Models;
using BattleShips.Web.Services.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace BattleShips.Web.Controllers
{
    public class GameController : Controller
    {
        private const string WarningGameRestarted = "Game needed to be restarted. Please start again";
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public GameController(IServiceProvider serviceProvider, ILogger<GameController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(GameModel gameModel)
        {
            if (TempData[nameof(UserCommunicationViewModel.MessageToUser)] is string message)
            {
                gameModel.UserCommunicationVM.MessageToUser.Add(message);
            }

            if (TempData[nameof(UserShipsLocationViewModel)] is string serializedVm)
            {
                var userShipsLocationVM = JsonConvert.DeserializeObject<UserShipsLocationViewModel>(serializedVm);
                gameModel.ShipsFields = userShipsLocationVM?.ShipsFields;
            }

            return View(gameModel);
        }

        public IActionResult CheckShips(UserShipsLocationViewModel userShipsLocationVM)
        {
            try
            {
                var shipLayouts = userShipsLocationVM.ShipsLayouts;
                var gameService = _serviceProvider.GetService<IGameService>();
                var fields = gameService.GetShipsPositions(shipLayouts);
                userShipsLocationVM.ShipsFields = fields;
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"General error in {nameof(CheckShips)} method");
                TempData[nameof(UserCommunicationViewModel.MessageToUser)] = "Error: " + e.Message;
            }

            TempData.AddSerializedObject(userShipsLocationVM, nameof(UserShipsLocationViewModel));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Create(GameModel gameModel, UserShipsLocationViewModel userShipsLocationVM)
        {
            // Guid has not been created yet
            ModelState.ClearValidationState(nameof(GameModel.GameGuid));
            ModelState.MarkFieldSkipped(nameof(GameModel.GameGuid));
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Model was not validated in {nameof(Create)} method");
                TempData[nameof(UserCommunicationViewModel.MessageToUser)] = WarningGameRestarted;
                return View(nameof(Index), gameModel);
            }

            IGameService gameService = null;
            try
            {
                gameService = _serviceProvider.GetService<IGameService>();
                gameModel.Game = gameService.InitializeGame(userShipsLocationVM.ShipsLayouts, new DifficultyLevelEasy());
                gameModel.GameGuid = gameService.CurrentGameGuid;

                return View("GameProgress", gameModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"General error in {nameof(Create)} method");

                if (gameService != null)
                    gameService.RemoveGame(gameModel.GameGuid);

                TempData[nameof(UserCommunicationViewModel.MessageToUser)] = "Error: " + e.Message;
                TempData.AddSerializedObject(userShipsLocationVM, nameof(UserShipsLocationViewModel));

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult NextTurn(int? shootPositionX, int? shootPositionY, GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var gameService = _serviceProvider.GetService<IGameService>();
                    var result = gameService.TakeNextRound(shootPositionX.Value, shootPositionY.Value, gameModel.GameGuid.Value);

                    var userCommunicationVM = gameModel.UserCommunicationVM;
                    userCommunicationVM.CurrentGameStatus = result;

                    gameModel.Game = gameService.CurrentGame;

                    return View("GameProgress", gameModel);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"General error in {nameof(NextTurn)} method");
                    TempData[nameof(UserCommunicationViewModel.MessageToUser)] = "Error: " + e.Message;
                }
            }
            else
            {
                _logger.LogError($"Model was not validated in {nameof(NextTurn)} method");
                TempData[nameof(UserCommunicationViewModel.MessageToUser)] = WarningGameRestarted;
            }

            return RedirectToAction(nameof(Index));
        }

    }
}