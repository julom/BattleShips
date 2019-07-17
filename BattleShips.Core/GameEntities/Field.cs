using BattleShips.Core.Exceptions;
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

        public bool Shoot()
        {
            if (FieldType == FieldTypeEnum.MissedShot || FieldType == FieldTypeEnum.ShipHit)
            {
                throw new GameLogicalException("Field has been already shot");
            }

            if (FieldType == FieldTypeEnum.Empty)
            {
                FieldType = FieldTypeEnum.MissedShot;
                return false;
            }

            if (FieldType == FieldTypeEnum.Ship)
            {
                FieldType = FieldTypeEnum.ShipHit;
                return true;
            }

            throw new GameArgumentException("Field has unexpected type");
        }
    }
}
