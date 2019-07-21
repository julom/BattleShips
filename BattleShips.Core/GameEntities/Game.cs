using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;

namespace BattleShips.Core.GameEntities
{
    public class Game : IGame
    {
        private readonly IDifficultyLevel difficulty;

        public Board[] Boards => throw new System.NotImplementedException();

        public bool IsWon => throw new System.NotImplementedException();

        public bool IsLost => throw new System.NotImplementedException();

        public Game(IDifficultyLevel difficulty)
        {

        }

        public IField[,] MakeComputerMovement()
        {
            throw new System.NotImplementedException();
        }

        public IField[,] MakePlayerMovement(int shotPositionX, int shotPositionY)
        {
            throw new System.NotImplementedException();
        }
    }
}
