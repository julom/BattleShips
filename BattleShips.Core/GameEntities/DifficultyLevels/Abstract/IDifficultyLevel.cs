using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.GameEntities.DifficultyLevels.Abstract
{
    public interface IDifficultyLevel
    {
        KeyValuePair<int, int> ChooseShotPosition();
    }
}
