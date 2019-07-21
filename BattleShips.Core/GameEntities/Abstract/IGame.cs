using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IGame
    {
        Board[] Boards { get; }
        bool IsWon { get; }
        bool IsLost { get; }

        IField[,] MakePlayerMovement(int shotPositionX, int shotPositionY);
        IField[,] MakeComputerMovement();
    }
}
