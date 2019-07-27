using BattleShips.Core;
using BattleShips.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BattleShips.Web.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var gameModel = new GameModel();
            return View(gameModel);
        }

        [HttpPost]
        public IActionResult Create(GameModel gameModel)
        {
            gameModel.InitializeGame();

            return View("GameProgress", gameModel);
        }
    }
}