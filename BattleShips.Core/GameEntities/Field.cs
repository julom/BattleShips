using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Enums;
using System;

namespace BattleShips.Core.GameEntities
{
    public class Field : IField
    {
        public int PositionX { get; }

        public int PositionY { get; }

        public FieldTypes FieldType { get; private set; }

        public Field(FieldTypes fieldType, int positionX, int positionY)
        {
            FieldType = fieldType;
            PositionX = positionX;
            PositionY = positionY;
        }

        public bool Shoot()
        {
            if (FieldType == FieldTypes.MissedShot || FieldType == FieldTypes.ShipHit)
            {
                throw new GameLogicalException("Field has been already shot");
            }

            if (FieldType == FieldTypes.Empty)
            {
                FieldType = FieldTypes.MissedShot;
                return false;
            }

            if (FieldType == FieldTypes.Ship)
            {
                FieldType = FieldTypes.ShipHit;
                return true;
            }

            throw new GameArgumentException("Field has unexpected type");
        }
    }
}
