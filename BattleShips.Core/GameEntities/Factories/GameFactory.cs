using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;

namespace BattleShips.Core.GameEntities.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IBoardFactory _boardFactory;

        public GameFactory(IBoardFactory boardFactory)
        {
            _boardFactory = boardFactory;
        }

        public IGame Create(IShip[] playerShips, IDifficultyLevel difficulty)
        {
            return new Game(playerShips, _boardFactory, difficulty);
        }
    }
}
