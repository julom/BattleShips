using BattleShips.Core;
using BattleShips.Core.GameEntities.Abstract;

namespace BattleShips.Web.Models
{
    public class GameModel
    {
        public readonly int SizeX = GameSettings.BoardSizeX;
        public readonly int SizeY = GameSettings.BoardSizeY;


        public IGame Game { get; set; }
        public bool[,] PlayerShipsPositions { get; set; }
        public ShootResultDTO LastShootResult { get; set; }

    }
}
