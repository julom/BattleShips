using System;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core;
using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Factories;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.GameRepository;
using BattleShips.Web.Services.Abstract;

namespace BattleShips.Web.Services
{
    public class GameService : IGameService
    {
        private readonly IGameSettings _gameSettings;
        private readonly IGameRepository _gameRepository;
        private readonly IShipFactory _shipFactory;

        public IGame CurrentGame { get; private set; }
        public Guid? CurrentGameGuid { get => CurrentGame?.Guid; }

        public GameService(IGameSettings gameSettings, IGameRepository gameRepository, IShipFactory shipFactory)
        {
            _gameSettings = gameSettings;
            _gameRepository = gameRepository;
            _shipFactory = shipFactory;
        }

        public bool[] TryShipPositioning(IList<ShipLayout> shipsLayouts)
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
                    shipPositions[row * _gameSettings.BoardSizeX + col] = allShipsFields.Any(x => x.PositionX == col && x.PositionY == row);
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

        public bool RemoveGame()
        {
            if (CurrentGameGuid.HasValue)
            {
                return _gameRepository.DeleteGame(CurrentGameGuid.Value);
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
            UpdateGameStatus(currentGameStatus, "You", playerResult, CurrentGame);

            if (!CurrentGame.IsWon)
            {
                var computerResult = CurrentGame.MakeComputerMovement();
                UpdateGameStatus(currentGameStatus, "Opponent", computerResult, CurrentGame);
            }

            _gameRepository.UpdateGame(gameGuid, CurrentGame);

            return currentGameStatus;
        }

        private static void UpdateGameStatus(IList<string> gameStatusList, string person, ShootResultDTO shootResult, IGame game)
        {
            if (gameStatusList == null) gameStatusList = new List<string>();

            var shotCoordinate = new Coordinate(shootResult.PositionX, shootResult.PositionY);
            var boardShotPosition = Coordinate.GetBoardPosition(shotCoordinate);

            gameStatusList.Add($"{person} shot position ({boardShotPosition})");
            gameStatusList.Add(shootResult.IsShipHit ? "Hit!" : "Missed");

            if (shootResult.IsShipSunk)
            {
                gameStatusList.Add("Ship is sunk!");
                if (game.IsWon)
                {
                    gameStatusList.Add("You sunk all opponents ships, you won!");
                }
                else if (game.IsLost)
                {
                    gameStatusList.Add("Opponent sunk all your ships, you lost");
                }
            }
        }
    }
}
