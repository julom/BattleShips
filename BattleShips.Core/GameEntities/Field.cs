using BattleShips.Core.GameEntities.Abstract;
using System;

namespace BattleShips.Core.GameEntities
{
    public class Field : IField
    {
        public FieldTypeEnum FieldType { get; private set; }

        public Field(FieldTypeEnum fieldType)
        {
            FieldType = fieldType;
        }

        public void Shoot(bool success)
        {
            throw new NotImplementedException();
        }
    }
}
