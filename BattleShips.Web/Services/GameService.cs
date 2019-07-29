using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Core;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.GameRepository;
using BattleShips.Web.Models;

namespace BattleShips.Web.Services
{
    public interface IGameService
    {
        IGame CurrentGame { get; }

        void InitializeGame(bool[] PlayerShipsPositions);
        void TakeNextRound(int shootPositionX, int shootPositionY);
    }

    public class GameService : IGameService
    {
        private readonly IGameSettings _gameSettings;
        private readonly IGameRepository _gameRepository;
        private Guid _currentGameGuid;

        public IGame CurrentGame { get; private set; }
        
        public GameService(IGameSettings gameSettings, IGameRepository gameRepository)
        {
            _gameSettings = gameSettings;
            _gameRepository = gameRepository;
        }

        public void InitializeGame(bool[] PlayerShipsPositions)
        {
            var shipPositions = new bool[_gameSettings.BoardSizeX, _gameSettings.BoardSizeY];
            for (int row = 0; row < _gameSettings.BoardSizeY; row++)
            {
                for (int col = 0; col < _gameSettings.BoardSizeX; col++)
                {
                    shipPositions[row, col] = PlayerShipsPositions[row * _gameSettings.BoardSizeX + col];
                }
            }

            _currentGameGuid = Guid.NewGuid();
            CurrentGame = _gameRepository.CreateGame(_currentGameGuid, shipPositions, new Core.GameEntities.DifficultyLevels.DifficultyLevelEasy());
        }

        public void TakeNextRound(int shootPositionX, int shootPositionY)
        {
            CurrentGame = _gameRepository.GetGame(_currentGameGuid);

            var playerResult = CurrentGame.MakePlayerMovement(shootPositionX, shootPositionY);
            UserCommunicationViewModel UserCommunicationVM = null;
            UpdateGameStatus(UserCommunicationVM.CurrentGameStatus, "You", playerResult, CurrentGame);

            if (!CurrentGame.IsWon)
            {
                var computerResult = CurrentGame.MakeComputerMovement();
                UpdateGameStatus(UserCommunicationVM.CurrentGameStatus, "Opponent", computerResult, CurrentGame);
            }

            _gameRepository.UpdateGame(_currentGameGuid, CurrentGame);
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
