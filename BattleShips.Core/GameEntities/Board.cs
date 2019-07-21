using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.GameEntities
{
    public class Board : IBoard
    {
        public IShip[] Ships { get; private set; }

        public IField[,] Fields { get; private set; }

        public Board(bool[,] fields)
        {
            Fields = FillTheFields(fields);
            Ships = DefineShipsPositions(fields);
        }

        private IField[,] FillTheFields(bool[,] fields)
        {
            IField[,] result = new IField[10, 10];

            for (int row = 0; row < fields.GetLength(0); row++)
            {
                for (int col = 0; col < fields.GetLength(1); col++)
                {
                    if (fields[row, col])
                    {
                        var shipField = new KeyValuePair<int, int>(row, col);
                        result[row, col] = new Field(FieldTypeEnum.Ship);
                    }
                    else
                    {
                        result[row, col] = new Field(FieldTypeEnum.Empty);
                    }
                }
            }

            return result;
        }

        public void RandomizeShipsPositions(IList<int> shipSizes)
        {
            throw new System.NotImplementedException();
        }

        public IShip[] DefineShipsPositions(bool[,] fields)
        {
            var ships = new List<Ship>();
            var shipsFields = new List<List<KeyValuePair<int, int>>>(GameSettings.ShipSizes.Count);

            for (int row = 0; row < fields.GetLength(1); row++)
            {
                for (int col = 0; col < fields.GetLength(0); col++)
                {
                    if (fields[row, col])
                    {
                        var shipField = new KeyValuePair<int, int>(row, col);
                        var matchedShip = shipsFields.FirstOrDefault(ship => ship != null && CheckIfFieldBelongsToShip(shipField, ship));
                        if (matchedShip != null)
                        {
                            matchedShip.Add(shipField);
                        }
                        else
                        {
                            shipsFields.Add(new List<KeyValuePair<int, int>>() { shipField });
                        }
                    }
                }
            }

            if (shipsFields.Count > GameSettings.ShipSizes.Count)
            {
                throw new GameArgumentException("Ship count exceeded");
            }

            foreach (var shipFields in shipsFields)
            {
                var ship = new Ship(shipFields, new ShipCoordinatesValidator());
                ships.Add(ship);
            }            

            return ships.ToArray();
        }

        private bool CheckIfFieldBelongsToShip(KeyValuePair<int, int> field, IList<KeyValuePair<int, int>> shipFields)
        {
            if (shipFields == null) return false;

            if (shipFields.Any(x=>x.Key + 1 == field.Key || x.Value + 1 == field.Value))
            {
                return true;
            }
            return false;
        }

        public bool Shoot(int positionX, int positionY)
        {
            var hit = Fields[positionY, positionX].Shoot();
            if (hit)
            {
                var foundShip = false;
                var shipIterator = 0;
                while (!foundShip)
                {
                    foundShip = Ships[shipIterator].TryToShoot(positionX, positionY);
                }
            }
            return hit;
        }
    }
}
