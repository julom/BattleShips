using System;
using BattleShips.Core;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;

namespace BattleShips.GameRepository
{
    public interface IGameRepository
    {
        IGame GetGame(Guid id);
        IGame CreateGame(bool[,] playerFields, IDifficultyLevel difficulty);
        IGame UpdateGame(Guid id, IGame game);
        void DeleteGame(Guid id);
    }
}