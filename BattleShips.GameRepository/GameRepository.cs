using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Factories;
using System;
using System.Collections.Generic;

namespace BattleShips.GameRepository
{
    public class GameRepository
    {
        private GameRepository() { }

        public static GameRepository Instance { get; } = new GameRepository();


        private Dictionary<Guid, IGame> _gameDictionary = new Dictionary<Guid, IGame>();

        public IGame GetGame(Guid id)
        {
            IGame game = null;
            _gameDictionary.TryGetValue(id, out game);
            return game;
        }

        public IGame CreateGame(Guid id, bool[,] playerFields, IDifficultyLevel difficulty)
        {
            IGame game = new Game(playerFields, new BoardFactory(), difficulty);
            _gameDictionary.Add(id, game);
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
