﻿using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;
using System;

namespace BattleShips.Core.GameEntities
{
    public class Game : IGame
    {
        private readonly IDifficultyLevel _difficulty;

        public Guid Guid { get; }

        public IBoard PlayerBoard { get; private set; }

        public IBoard ComputerBoard { get; private set; }

        public bool IsWon
        {
            get { return ComputerBoard.AreAllShipsSunk; }
        }

        public bool IsLost
        {
            get { return PlayerBoard.AreAllShipsSunk; }
        }


        public Game(IShip[] playerShips, IBoardFactory boardFactory, IDifficultyLevel difficulty)
        {
            _difficulty = difficulty;
            Guid = Guid.NewGuid();
            PlayerBoard = boardFactory.CreateBoard(playerShips);
            ComputerBoard = boardFactory.CreateBoard();
        }

        public ShootResultDTO MakeComputerMovement()
        {
            CheckIfGameEnded();

            var shotCoordinates = _difficulty.ChooseShotCoordinates(PlayerBoard);

            var result = PlayerBoard.Shoot(shotCoordinates.Key, shotCoordinates.Value);
            return result;
        }

        public ShootResultDTO MakePlayerMovement(int shotPositionX, int shotPositionY)
        {
            CheckIfGameEnded();

            var result = ComputerBoard.Shoot(shotPositionX, shotPositionY);
            return result;
        }

        private void CheckIfGameEnded()
        {
            if (IsLost || IsWon)
            {
                var endGameResult = IsWon ? "won" : "lost";
                throw new GameLogicalException($"Game ended. You {endGameResult}. Please start new game.");
            }
        }
    }
}
