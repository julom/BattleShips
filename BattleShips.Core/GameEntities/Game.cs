using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;

namespace BattleShips.Core.GameEntities
{
    public class Game : IGame
    {
        private readonly IDifficultyLevel difficulty;

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


        public Game(bool[,] playerFields, IBoardFactory boardFactory, IDifficultyLevel difficulty)
        {
            PlayerBoard = boardFactory.CreateBoard(playerFields);
            ComputerBoard = boardFactory.CreateBoard();
            this.difficulty = difficulty;
        }

        public ShootResultDTO MakeComputerMovement()
        {
            var shotCoordinates = difficulty.ChooseShotCoordinates(PlayerBoard);

            var result = PlayerBoard.Shoot(shotCoordinates.Key, shotCoordinates.Value);
            return result;
        }

        public ShootResultDTO MakePlayerMovement(int shotPositionX, int shotPositionY)
        {
            var result = ComputerBoard.Shoot(shotPositionX, shotPositionY);
            return result;
        }
    }
}
