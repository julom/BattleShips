using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Core;
using Microsoft.AspNetCore.Mvc;

namespace BattleShips.Web.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            ViewData["RowsNumber"] = GameSettings.BoardSizeY;
            ViewData["ColumnsNumber"] = GameSettings.BoardSizeX;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}