using System;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels.Abstract;
using BattleShips.Core.GameEntities.Enums;

namespace BattleShips.Core.GameEntities.DifficultyLevels
{
    public class DifficultyLevelEasy : IDifficultyLevel
    {
        private readonly Random random = new Random();

        public KeyValuePair<int, int> ChooseShotCoordinates(IBoard board)
        {
            var fields2DArray = board.Fields;
            var fields = fields2DArray.Cast<IField>().ToArray();

            var notShotFields = fields.Where(x => x.FieldType == FieldTypes.Empty || x.FieldType == FieldTypes.Ship).ToArray();
            var randomNumber = random.Next(notShotFields.Length);

            var shotPositionX = notShotFields[randomNumber].PositionX;
            var shotPositionY = notShotFields[randomNumber].PositionY;

            return new KeyValuePair<int, int>(shotPositionX, shotPositionY);
        }
    }
}
