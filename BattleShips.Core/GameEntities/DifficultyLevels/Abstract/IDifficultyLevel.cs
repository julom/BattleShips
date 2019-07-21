using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;

namespace BattleShips.Core.GameEntities.DifficultyLevels.Abstract
{
    public interface IDifficultyLevel
    {
        KeyValuePair<int, int> ChooseShotCoordinates(IBoard board);
    }
}
