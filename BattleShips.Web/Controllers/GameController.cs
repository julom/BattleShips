using BattleShips.Core;
using BattleShips.Core.GameEntities;
using BattleShips.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using BattleShips.GameRepository;
using BattleShips.Web.Services;
using Microsoft.Extensions.DependencyInjection;

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
        public IActionResult Index()
        {
            var gameModel = new GameModel();

            var message = TempData["messageToUser"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                gameModel.UserCommunicationVM.MessageToUser.Add(message);
            }

            return View(gameModel);
        }

        [HttpPost]
        public IActionResult Index(GameModel gameModel)
        {
            try
            {
                var gameService = _serviceProvider.GetService<IGameService>();
                gameService.InitializeGame(gameModel.PlayerShipsPositions);
                gameModel.CurrentGame = gameService.CurrentGame;
            }
            catch(Exception e)
            {
                gameModel.UserCommunicationVM.MessageToUser.Add(e.Message);
                return View(gameModel);
            }

            return View("GameProgress", gameModel);
        }

        public IActionResult NextTurn(int? shootPositionX, int? shootPositionY, GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var gameService = _serviceProvider.GetService<IGameService>();
                    gameService.TakeNextRound(shootPositionX.Value, shootPositionY.Value);
                    gameModel.CurrentGame = gameService.CurrentGame;
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