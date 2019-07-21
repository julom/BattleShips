using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Factories.Abstract;

namespace BattleShips.Core.GameEntities.Factories
{
    public class BoardFactory : IBoardFactory
    {
        public IBoard CreateBoard(bool[,] fields = null)
        {
            if (fields == null)
            {
                return new Board();
            }
            else
            {
                return new Board(fields);
            }
        }
    }
}
