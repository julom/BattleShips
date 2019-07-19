using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips.Core.GameEntities.Abstract
{
    public class FieldPositionWrapper : IFieldPositionWrapper
    {
        public IField Field { get; }
        public int PositionX { get; }
        public int PositionY { get; }

        public FieldPositionWrapper(IField field, int positionX, int positionY)
        {
            Field = field;
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}
