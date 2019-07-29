using BattleShips.Core;
using BattleShips.Core.GameEntities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BattleShips.Web.Models
{
    public class GameModel
    {
        public readonly int SizeX = GameSettings.BoardSizeX;
        public readonly int SizeY = GameSettings.BoardSizeY;

        public IGame CurrentGame { get; set; }

        [Required]
        public Guid CurrentGameGuid { get; set; }

        public bool[] PlayerShipsPositions
        {
            get;
            set;
        }
        public ShootResultDTO LastShootResult { get; }

        public UserCommunicationViewModel UserCommunicationVM { get; } = new UserCommunicationViewModel();

        public GameModel()
        {
            PlayerShipsPositions = new bool[SizeX * SizeY];
        }

        public void InitializeGame()
        {
            var shipPositions = new bool[SizeX, SizeY];
            for (int row = 0; row < SizeY; row++)
            {
                for (int col = 0; col < SizeX; col++)
                {
                    shipPositions[row, col] = PlayerShipsPositions[row * SizeX + col];
                }
            }

            CurrentGameGuid = Guid.NewGuid();
            CurrentGame = GameRepository.GameRepository.Instance.CreateGame(CurrentGameGuid, shipPositions, new Core.GameEntities.DifficultyLevels.DifficultyLevelEasy());
        }

        public void TakeNextRound(int shootPositionX, int shootPositionY)
        {
            CurrentGame = GameRepository.GameRepository.Instance.GetGame(CurrentGameGuid);

            var playerResult = CurrentGame.MakePlayerMovement(shootPositionX, shootPositionY);
            UpdateGameStatus(UserCommunicationVM.CurrentGameStatus, "You", playerResult, CurrentGame);

            if (!CurrentGame.IsWon)
            {
                var computerResult = CurrentGame.MakeComputerMovement();
                UpdateGameStatus(UserCommunicationVM.CurrentGameStatus, "Opponent", computerResult, CurrentGame);
            }

            GameRepository.GameRepository.Instance.UpdateGame(CurrentGameGuid, CurrentGame);
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
