using BattleShips.Core;
using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;

namespace BattleShips.Web.Models
{
    public class GameModel
    {
        public readonly int SizeX = GameSettings.BoardSizeX;
        public readonly int SizeY = GameSettings.BoardSizeY;


        public Game Game { get; set; }
        public bool[] PlayerShipsPositions
        {
            get;
            set;
        }
        public ShootResultDTO LastShootResult { get; set; }

        public GameModel()
        {
            PlayerShipsPositions = new bool[SizeX * SizeY];
        }

        public void InitializeGame()
        {
            var shipPositions = new bool[SizeX, SizeY];
            for (int row = 0; row < SizeY; row++)
            {
                for (int col = 0; col < SizeX; col++)
                {
                    shipPositions[row, col] = PlayerShipsPositions[row * SizeX + col];
                }
            }

            Game = new Game(shipPositions, new Core.GameEntities.Factories.BoardFactory(), new Core.GameEntities.DifficultyLevels.DifficultyLevelEasy());
        }
    }
}
