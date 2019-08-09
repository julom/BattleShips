using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;
using BattleShips.Core.GameEntities.Utils.Abstract;
using BattleShips.Core.GameEntities.Validators.Abstract;
using System;

namespace BattleShips.Core.GameEntities.Factories
{
    public class BoardFactory : IBoardFactory
    {
        private readonly IGameSettings _gameSettings;
        private readonly IShipPositionsRandomizer _shipPositionsRandomizer;
        private readonly IShipsGroupValidator _shipsGroupValidator;

        public BoardFactory(IGameSettings gameSettings, IShipPositionsRandomizer shipPositionsRandomizer, IShipsGroupValidator shipsGroupValidator)
        {
            _gameSettings = gameSettings;
            _shipPositionsRandomizer = shipPositionsRandomizer;
            _shipsGroupValidator = shipsGroupValidator;
        }

        // Create board for player
        public IBoard CreateBoard(IShip[] ships)
        {
            return new Board(ships, _shipsGroupValidator, _gameSettings);
        }

        // Create board for computer opponent
        public IBoard CreateBoard()
        {
            return new Board(_shipPositionsRandomizer, _gameSettings);
        }
    }
}
