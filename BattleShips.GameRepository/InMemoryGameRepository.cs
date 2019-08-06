using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using System;
using System.Collections.Generic;
using BattleShips.Core.GameEntities.Factories.Abstract;

namespace BattleShips.GameRepository
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly IBoardFactory _boardFactory;
        private readonly IDictionary<Guid, IGame> _gameDictionary = new Dictionary<Guid, IGame>();
        
        public InMemoryGameRepository(IBoardFactory boardFactory)
        {
            _boardFactory = boardFactory;
        }

        public IGame GetGame(Guid id)
        {
            IGame game = null;
            _gameDictionary.TryGetValue(id, out game);
            return game;
        }

        public IGame CreateGame(IShip[] playerShips, IDifficultyLevel difficulty)
        {
            IGame game = new Game(playerShips, _boardFactory, difficulty);
            _gameDictionary.Add(game.Guid, game);
            return game;
        }

        public IGame UpdateGame(Guid id, IGame game)
        {
            IGame existingGame = null;
            _gameDictionary.TryGetValue(id, out existingGame);
            existingGame = game;
            return game;
        }

        public void DeleteGame(Guid id)
        {
            if (_gameDictionary.ContainsKey(id))
            {
                _gameDictionary.Remove(id);
            }
        }
    }
}
