namespace BattleShips.Core.GameEntities.Abstract
{
    public interface IGame
    {
        IBoard PlayerBoard { get; }
        IBoard ComputerBoard { get; }
        bool IsWon { get; }
        bool IsLost { get; }

        ShootResultDTO MakePlayerMovement(int shotPositionX, int shotPositionY);
        ShootResultDTO MakeComputerMovement();
    }
}
