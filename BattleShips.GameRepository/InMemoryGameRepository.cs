using System;
using System.Collections.Generic;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;

namespace BattleShips.GameRepository
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly IGameFactory _gameFactory;
        private readonly IDictionary<Guid, IGame> _gameDictionary = new Dictionary<Guid, IGame>();
        
        public InMemoryGameRepository(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public IGame GetGame(Guid id)
        {
            _gameDictionary.TryGetValue(id, out IGame game);
            return game;
        }

        public IGame CreateGame(IShip[] playerShips, IDifficultyLevel difficulty)
        {
            IGame game = _gameFactory.Create(playerShips, difficulty);
            _gameDictionary.Add(game.Guid, game);
            return game;
        }

        public IGame UpdateGame(Guid id, IGame game)
        {
            if (_gameDictionary.ContainsKey(id))
            {
                _gameDictionary[id] = game;
            }

            return game;
        }

        public bool DeleteGame(Guid id)
        {
            if (_gameDictionary.ContainsKey(id))
            {
                return _gameDictionary.Remove(id);
            }
            return false;
        }
    }
}
