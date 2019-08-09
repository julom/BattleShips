using System;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;

namespace BattleShips.GameRepository
{
    public interface IGameRepository
    {
        IGame GetGame(Guid id);
        IGame CreateGame(IShip[] playerShips, IDifficultyLevel difficulty);
        IGame UpdateGame(Guid id, IGame game);
        bool DeleteGame(Guid id);
    }
}