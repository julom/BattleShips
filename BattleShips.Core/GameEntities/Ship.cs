using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core.Exceptions;

namespace BattleShips.Core.GameEntities
{
    public class Ship : IShip
    {
        private readonly IShipCoordinatesValidator coordinatesValidator;
        public int Size { get; private set; }
        public IList<IFieldPositionWrapper> Coordinates { get; private set; }

        public bool IsSunk
        {
            get { return Coordinates.All(x => x.Field.FieldType == FieldTypeEnum.ShipHit); }
        }

        public Ship(IList<KeyValuePair<int, int>> coordinates, IShipCoordinatesValidator coordinatesValidator)
        {
            this.coordinatesValidator = coordinatesValidator;
            Size = coordinates.Count;
            Coordinates = GetCoordinates(coordinates);
        }

        private List<IFieldPositionWrapper> GetCoordinates(IList<KeyValuePair<int, int>> coordinates)
        {
            if (coordinatesValidator.Validate(coordinates))
            {
                return coordinates.Select(x =>
                        (IFieldPositionWrapper)new FieldPositionWrapper(new Field(FieldTypeEnum.Ship), x.Key, x.Value))
                    .ToList();
            }
            throw new GameArgumentException("Wrong ship coordinates");
        }

        public bool TryToShoot(int positionX, int positionY)
        {
            var fieldToShoot = Coordinates.FirstOrDefault(x => x.PositionX == positionX && x.PositionY == positionY);
            if (fieldToShoot != null)
            {
                return fieldToShoot.Field.Shoot();
            }
            return false;
        }
    }
}
