using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;

namespace BattleShips.Core.GameEntities.Factories.Abstract
{
    public interface IGameFactory
    {
        IGame Create(IShip[] playerShips, IDifficultyLevel difficulty);
    }
}
