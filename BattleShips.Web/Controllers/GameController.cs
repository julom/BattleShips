using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using BattleShips.Web.Models;
using BattleShips.Web.Services.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels;
using System;
using Newtonsoft.Json;

namespace BattleShips.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public GameController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                TempData[nameof(UserCommunicationViewModel.MessageToUser)] = "Error: " + e.Message;
            }

            var serializedVm = JsonConvert.SerializeObject(userShipsLocationVM);
            TempData[nameof(UserShipsLocationViewModel)] = serializedVm;

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
                TempData[nameof(UserCommunicationViewModel.MessageToUser)] = "Game needed to be restarted. Please start again";
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
                if (gameService != null)
                    gameService.RemoveGame(gameModel.GameGuid);

                TempData[nameof(UserCommunicationViewModel.MessageToUser)] = "Error: " + e.Message;

                var serializedVm = JsonConvert.SerializeObject(userShipsLocationVM);
                TempData[nameof(UserShipsLocationViewModel)] = serializedVm;
            }

            return RedirectToAction(nameof(Index));
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
                    TempData[nameof(UserCommunicationViewModel.MessageToUser)] = "Error: " + e.Message;
                }
            }
            else
            {
                TempData[nameof(UserCommunicationViewModel.MessageToUser)] = "Game needed to be restarted. Please start again";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}