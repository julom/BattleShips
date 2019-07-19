using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;
using System.Linq;
using BattleShips.Core.Exceptions;

namespace BattleShips.Core.GameEntities
{
    public class Ship : IShip
    {
        public int Size { get; private set; }
        public IList<IFieldPositionWrapper> Cooridnates { get; private set; }

        public bool IsSunk
        {
            get { return Cooridnates.All(x => x.Field.FieldType == FieldTypeEnum.ShipHit); }
        }

        public Ship(IList<KeyValuePair<int, int>> coordinates)
        {
            Cooridnates = GetCoordinates(coordinates);
            Size = Cooridnates.Count;
        }

        private static List<IFieldPositionWrapper> GetCoordinates(IList<KeyValuePair<int, int>> coordinates)
        {
            return coordinates.Select(x =>
                    (IFieldPositionWrapper) new FieldPositionWrapper(new Field(FieldTypeEnum.Ship), x.Key, x.Value))
                .ToList();
        }

        public bool TryToShoot(int positionX, int positionY)
        {
            var fieldToShoot = Cooridnates.FirstOrDefault(x => x.PositionX == positionX && x.PositionY == positionY);
            if (fieldToShoot != null)
            {
                return fieldToShoot.Field.Shoot();
            }
            return false;
        }
    }
}
