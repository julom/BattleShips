using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Structs;
using System;
using System.Collections.Generic;

namespace BattleShips.Web.Services.Abstract
{
    public interface IGameService
    {
        IGame CurrentGame { get; }
        Guid? CurrentGameGuid { get; }

        bool[] TryShipPositioning(IList<KeyValuePair<ShipVector, ShipVector>> shipsVectors);
        IGame InitializeGame(IList<KeyValuePair<ShipVector, ShipVector>> shipsVectors, IDifficultyLevel difficultyLevel);
        IList<string> TakeNextRound(int shootPositionX, int shootPositionY, Guid gameGuid);
    }
}
