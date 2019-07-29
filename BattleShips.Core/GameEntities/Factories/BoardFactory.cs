using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;

namespace BattleShips.Core.GameEntities.Factories
{
    public class BoardFactory : IBoardFactory
    {
        private readonly IGameSettings _gameSettings;
        private readonly IShipFactory _shipFactory;

        public BoardFactory(IGameSettings gameSettings, IShipFactory shipFactory)
        {
            _gameSettings = gameSettings;
            _shipFactory = shipFactory;
        }

        public IBoard CreateBoard(bool[,] fields = null)
        {
            if (fields == null)
            {
                return new Board(_gameSettings, _shipFactory);
            }
            else
            {
                return new Board(fields, _gameSettings, _shipFactory);
            }
        }
    }
}
