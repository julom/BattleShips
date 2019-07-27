using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Enums;
using BattleShips.Core.GameEntities.Structs;
using System;

namespace BattleShips.Core.GameEntities
{
    public class Field : IField
    {
        public Coordinate Position { get; }
        public int PositionX { get => Position.PositionX; }
        public int PositionY { get => Position.PositionY; }
        public FieldTypes FieldType { get; private set; } = FieldTypes.Empty;
        public bool IsShipField { get => FieldType == FieldTypes.Ship; }

        public Field() { }

        public Field(FieldTypes fieldType, int positionX, int positionY)
        {
            FieldType = fieldType;
            Position = new Coordinate(positionX, positionY);
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
