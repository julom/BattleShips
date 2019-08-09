using System;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core;
using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Factories;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.Core.GameEntities.Utils.Abstract;
using BattleShips.GameRepository;
using BattleShips.Web.Services.Abstract;

namespace BattleShips.Web.Services
{
    public class GameService : IGameService
    {
        private readonly IGameSettings _gameSettings;
        private readonly IGameRepository _gameRepository;
        private readonly IShipFactory _shipFactory;
        private readonly IGameStatusUpdater _gameStatusUpdater;

        public IGame CurrentGame { get; private set; }
        public Guid? CurrentGameGuid { get => CurrentGame?.Guid; }

        public GameService(IGameSettings gameSettings, IGameRepository gameRepository, IShipFactory shipFactory, IGameStatusUpdater gameStatusUpdater)
        {
            _gameSettings = gameSettings;
            _gameRepository = gameRepository;
            _shipFactory = shipFactory;
            _gameStatusUpdater = gameStatusUpdater;
        }

        public bool[] GetShipsPositions(IList<ShipLayout> shipsLayouts)
        {
            var allShipsFields = new List<IField>();
            foreach (var layout in shipsLayouts)
            {
                var ship = _shipFactory.Create(layout.VectorX, layout.VectorY);
                allShipsFields.AddRange(ship.Coordinates);
            }

            var shipPositions = new bool[_gameSettings.BoardSizeX * _gameSettings.BoardSizeY];
            for (int row = 0; row < _gameSettings.BoardSizeY; row++)
            {
                for (int col = 0; col < _gameSettings.BoardSizeX; col++)
                {
                    shipPositions[row * _gameSettings.BoardSizeY + col] = allShipsFields.Any(x => x.PositionX == col && x.PositionY == row);
                }
            }

            return shipPositions;
        }

        public IGame InitializeGame(IList<ShipLayout> shipsLayouts, IDifficultyLevel difficultyLevel)
        {
            var shipPositions = new List<IShip>();
            foreach (var layout in shipsLayouts)
            {
                var ship = _shipFactory.Create(layout.VectorX, layout.VectorY);
                shipPositions.Add(ship);
            }

            CurrentGame = _gameRepository.CreateGame(shipPositions.ToArray(), difficultyLevel);
            return CurrentGame;
        }

        public bool RemoveGame(Guid? gameGuid)
        {
            if (gameGuid.HasValue)
            {
                return _gameRepository.DeleteGame(gameGuid.Value);
            }
            return false;
        }

        public IList<string> TakeNextRound(int shootPositionX, int shootPositionY, Guid gameGuid)
        {
            var currentGameStatus = new List<string>();
            CurrentGame = _gameRepository.GetGame(gameGuid);

            if (CurrentGame == null)
                throw new GameException("Game not available. Please start a new game");

            var playerResult = CurrentGame.MakePlayerMovement(shootPositionX, shootPositionY);
            _gameStatusUpdater.UpdateGameStatus(currentGameStatus, "You", playerResult, CurrentGame);

            if (!CurrentGame.IsWon)
            {
                var computerResult = CurrentGame.MakeComputerMovement();
                _gameStatusUpdater.UpdateGameStatus(currentGameStatus, "Opponent", computerResult, CurrentGame);
            }

            _gameRepository.UpdateGame(gameGuid, CurrentGame);

            return currentGameStatus;
        }
    }
}
