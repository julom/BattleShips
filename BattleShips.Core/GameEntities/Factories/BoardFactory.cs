using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;
using BattleShips.Core.GameEntities.Utils.Abstract;
using System;

namespace BattleShips.Core.GameEntities.Factories
{
    public class BoardFactory : IBoardFactory
    {
        private readonly IGameSettings _gameSettings;
        private readonly IShipFactory _shipFactory;
        private readonly IShipPositionsRandomizer _shipPositionsRandomizer;

        public BoardFactory(IGameSettings gameSettings, IShipFactory shipFactory, IShipPositionsRandomizer shipPositionsRandomizer)
        {
            _gameSettings = gameSettings;
            _shipFactory = shipFactory;
            _shipPositionsRandomizer = shipPositionsRandomizer;
        }

        public IBoard CreateBoard(IShip[] ships = null)
        {
            if (ships == null)
            {
                return new Board(_gameSettings, _shipFactory, _shipPositionsRandomizer);
            }
            else
            {
                return new Board(ships, _gameSettings, _shipFactory);
                throw new NotImplementedException();
            }
        }
    }
}
