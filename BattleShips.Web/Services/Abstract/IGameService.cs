using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using System;
using System.Collections.Generic;

namespace BattleShips.Web.Services.Abstract
{
    public interface IGameService
    {
        IGame CurrentGame { get; }
        Guid? CurrentGameGuid { get; }

        IGame InitializeGame(bool[] PlayerShipsPositions, IDifficultyLevel difficultyLevel);
        IList<string> TakeNextRound(int shootPositionX, int shootPositionY, Guid gameGuid);
    }
}
