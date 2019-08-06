using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using BattleShips.Web.Models;
using BattleShips.Web.Services.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels;
using System;

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
            var message = TempData["messageToUser"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                gameModel.UserCommunicationVM.MessageToUser.Add(message);
            }

            return View(gameModel);
        }

        public IActionResult CheckShips(UserShipsLocationViewModel userShipsLocationVM)
        {
            var gameModel = new GameModel();

            try
            {
                var shipVectors = userShipsLocationVM.GetShipVectors();
                var gameService = _serviceProvider.GetService<IGameService>();
                var fields = gameService.TryShipPositioning(shipVectors);
                userShipsLocationVM.ShipsFields = fields;
                gameModel.PlayerShipsPositions = userShipsLocationVM;
            }
            catch(Exception e)
            {
                gameModel.UserCommunicationVM.MessageToUser.Add("Error: " + e.Message);
            }
            return View("Index", gameModel);
            return RedirectToAction(nameof(Index), gameModel);
        }

        [HttpPost]
        public IActionResult Start(GameModel gameModel)
        {
            try
            {
                var gameService = _serviceProvider.GetService<IGameService>();
                //gameModel.Game = gameService.InitializeGame(gameModel.PlayerShipsPositions, new DifficultyLevelEasy());
                gameModel.GameGuid = gameService.CurrentGameGuid;
            }
            catch(Exception e)
            {
                gameModel.UserCommunicationVM.MessageToUser.Add(e.Message);
                return View(gameModel);
            }

            return View("GameProgress", gameModel);
        }

        [HttpPost]
        public IActionResult NextTurn(int? shootPositionX, int? shootPositionY, GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var gameService = _serviceProvider.GetService<IGameService>();
                    gameService.TakeNextRound(shootPositionX.Value, shootPositionY.Value, gameModel.GameGuid.Value);
                    gameModel.Game = gameService.CurrentGame;
                }
                catch (Exception e)
                {
                    gameModel.UserCommunicationVM.MessageToUser.Add(e.Message);
                }

                return View("GameProgress", gameModel);
            }

            TempData["messageToUser"] = "Game needed to be restarted. Please start again";
            return RedirectToAction(nameof(Index));
        }

    }
}