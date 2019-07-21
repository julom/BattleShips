using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Enums;

namespace BattleShips.Core.GameEntities.Factories.Abstract
{
    public interface IBoardFactory
    {
        IBoard CreateBoard(bool[,] fields = null);
    }
}
