using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.Utils.Abstract
{
    public interface IGameStatusUpdater
    {
        void UpdateGameStatus(IList<string> gameStatusList, string person, ShootResultDTO shootResult, IGame game);
    }
}
