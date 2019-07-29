using System;
using System.Collections.Generic;
using BattleShips.Core;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.GameRepository;
using BattleShips.Web.Services.Abstract;

namespace BattleShips.Web.Services
{
    public class GameService : IGameService
    {
        private readonly IGameSettings _gameSettings;
        private readonly IGameRepository _gameRepository;

        public IGame CurrentGame { get; private set; }
        public Guid? CurrentGameGuid { get => CurrentGame?.Guid; }

        public GameService(IGameSettings gameSettings, IGameRepository gameRepository)
        {
            _gameSettings = gameSettings;
            _gameRepository = gameRepository;
        }

        public IGame InitializeGame(bool[] PlayerShipsPositions, IDifficultyLevel difficultyLevel)
        {
            var shipPositions = new bool[_gameSettings.BoardSizeX, _gameSettings.BoardSizeY];
            for (int row = 0; row < _gameSettings.BoardSizeY; row++)
            {
                for (int col = 0; col < _gameSettings.BoardSizeX; col++)
                {
                    shipPositions[row, col] = PlayerShipsPositions[row * _gameSettings.BoardSizeX + col];
                }
            }

            CurrentGame = _gameRepository.CreateGame(shipPositions, difficultyLevel);
            return CurrentGame;
        }

        public IList<string> TakeNextRound(int shootPositionX, int shootPositionY, Guid gameGuid)
        {
            var currentGameStatus = new List<string>();
            CurrentGame = _gameRepository.GetGame(gameGuid);

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

            gameStatusList.Add($"{person} shot position ({shootResult.PositionX},{shootResult.PositionY})");
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
