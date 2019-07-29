using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Enums;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.Core.GameEntities.Validators.Abstract;
using BattleShips.Core.GameEntities.Validators;

namespace BattleShips.Core.GameEntities
{
    public class Ship : IShip
    {
        private readonly IShipCoordinatesValidator coordinatesValidator;
        private readonly IShipVectorsValidator vectorsValidator;
        public int Size { get; private set; }
        public IList<IField> Coordinates { get; private set; }

        public bool IsSunk
        {
            get { return Coordinates.All(x => x.FieldType == FieldTypes.ShipHit); }
        }

        public Ship(ShipVector vectorX, ShipVector vectorY, IShipVectorsValidator vectorsValidator)
        {
            this.vectorsValidator = vectorsValidator;
            Coordinates = GetCoordinates(vectorX, vectorY);
            Size = Coordinates.Count;
        }

        public Ship(IList<KeyValuePair<int, int>> coordinates, IShipCoordinatesValidator coordinatesValidator)
        {
            this.coordinatesValidator = coordinatesValidator;
            Coordinates = GetCoordinates(coordinates);
            Size = Coordinates.Count;
        }

        private List<IField> GetCoordinates(ShipVector vectorX, ShipVector vectorY)
        {
            if (vectorsValidator.Validate(vectorX, vectorY))
            {
                var fieldLayout = new VectorLayout(vectorX, vectorY);
                return fieldLayout.Values.Select(x =>
                        (IField)new Field(FieldTypes.Ship, x.PositionX, x.PositionY))
                    .ToList();
            }
            throw new GameArgumentException("Wrong ship coordinates");
        }

        private List<IField> GetCoordinates(IList<KeyValuePair<int, int>> coordinates)
        {
            if (coordinatesValidator.Validate(coordinates))
            {
                return coordinates.Select(x =>
                        (IField)new Field(FieldTypes.Ship, x.Key, x.Value))
                    .ToList();
            }
            throw new GameArgumentException("Wrong ship coordinates");
        }

        public bool TryToShoot(int positionX, int positionY)
        {
            var fieldToShoot = Coordinates.FirstOrDefault(x => x.PositionX == positionX && x.PositionY == positionY);
            if (fieldToShoot != null)
            {
                return fieldToShoot.Shoot();
            }
            return false;
        }
    }
}
