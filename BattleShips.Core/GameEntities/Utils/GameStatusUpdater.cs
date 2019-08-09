using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.Core.GameEntities.Utils.Abstract;
using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Utils
{
    public class GameStatusUpdater : IGameStatusUpdater
    {
        public void UpdateGameStatus(IList<string> gameStatusList, string person, ShootResultDTO shootResult, IGame game)
        {
            if (shootResult == null) throw new GameArgumentException("Parameter is null", nameof(shootResult));
            if (game == null) throw new GameArgumentException("Parameter is null", nameof(game));
            if (gameStatusList == null) gameStatusList = new List<string>();

            var shotCoordinate = new Coordinate(shootResult.PositionX, shootResult.PositionY);
            var boardShotPosition = Coordinate.GetBoardPosition(shotCoordinate);

            gameStatusList.Add($"{person} shot position ({boardShotPosition})");
            gameStatusList.Add(shootResult.IsShipHit ? "Hit!" : "Missed");

            if (shootResult.IsShipSunk)
            {
                gameStatusList.Add("Ship is sunk!");
                if (game.IsWon)
                {
                    gameStatusList.Add("You sunk all opponents ships, you won!");
                }
                else if (game.IsLost)
                {
                    gameStatusList.Add("Opponent sunk all your ships, you lost");
                }
            }
        }
    }
}
