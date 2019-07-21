using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using System;
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
            Ships = DefineShipsPositions(fields);
            Fields = FillTheFields(Ships);
        }

        public Board()
        {
            RandomizeShipsPositions(GameSettings.ShipSizes);
            Fields = FillTheFields(Ships);
        }

        private IField[,] FillTheFields(IShip[] ships)
        {
            IField[,] outputFields = new Field[10, 10];

            foreach (var ship in ships)
            {
                foreach (var shipSegment in ship.Coordinates)
                {
                    outputFields[shipSegment.PositionX, shipSegment.PositionY] = new Field(FieldTypeEnum.Ship);
                }
            }
            
            for (int row = 0; row < GameSettings.BoardSizeX; row++)
            {
                for (int col = 0; col < GameSettings.BoardSizeY; col++)
                {
                    if (outputFields[row, col] == null)
                    {
                        outputFields[row, col] = new Field(FieldTypeEnum.Empty);
                    }
                }
            }
            
            return outputFields;
        }

        public void RandomizeShipsPositions(IList<int> shipSizes)
        {
            Fields = new IField[GameSettings.BoardSizeY, GameSettings.BoardSizeX];
            Ships = new IShip[GameSettings.ShipSizes.Count];

            List<PossibleShipPosition> realShipPositions = new List<PossibleShipPosition>();
            var random = new Random();

            var shipsLeft = new List<int>(GameSettings.ShipSizes);
            while (shipsLeft.Any())
            {
                var currentShipSize = shipsLeft.Max();

                List<PossibleShipPosition> possiblePositions = new List<PossibleShipPosition>();

                for (int row = 0; row < GameSettings.BoardSizeX; row++)
                {
                    for (int col = 0; col < GameSettings.BoardSizeY - currentShipSize; col++)
                    {
                        if (!realShipPositions.Any(x => x.OverlapsCurrentShip(row, col)))
                        {
                            possiblePositions.Add(new PossibleShipPosition() { Xfrom = row, Xto = row + currentShipSize, Yfrom = col, Yto = col });
                            possiblePositions.Add(new PossibleShipPosition() { Xfrom = row, Xto = row, Yfrom = col, Yto = col + currentShipSize });
                        }
                    }
                }

                var chosenPossiblePosition = possiblePositions[random.Next(possiblePositions.Count)];
                realShipPositions.Add(chosenPossiblePosition);

                shipsLeft.Remove(currentShipSize);
            }

            for (int i = 0; i < realShipPositions.Count; i++)
            {
                PossibleShipPosition ship = realShipPositions[i];
                List<KeyValuePair<int, int>> shipCoordinates = new List<KeyValuePair<int, int>>();

                for (int j = 0; j < ship.Size; j++)
                {
                    if (ship.SizeX > 0)
                        shipCoordinates.Add(new KeyValuePair<int, int>(ship.Xfrom + j, ship.Yfrom));
                    else
                        shipCoordinates.Add(new KeyValuePair<int, int>(ship.Xfrom, ship.Yfrom + j));
                }

                Ships[i] = new Ship(shipCoordinates);
            }
        }

        private class PossibleShipPosition
        {
            public int Xfrom;
            public int Xto;
            public int Yfrom;
            public int Yto;

            public int SizeX
            {
                get { return Xto - Xfrom; }
            }

            public int SizeY
            {
                get { return Yto - Yfrom; }
            }

            public int Size
            {
                get { return Math.Max(SizeX, SizeY); }
            }

            public bool OverlapsCurrentShip(int posX, int posY)
            {
                return (posX >= Xfrom && posX <= Xto &&
                    posY >= Yfrom && posY <= Yto);
            }
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

            if (shipFields.Any(x => x.Key + 1 == field.Key || x.Value + 1 == field.Value))
            {
                return true;
            }
            return false;
        }

        public ShootResultDTO Shoot(int positionX, int positionY)
        {
            ShootResultDTO result = new ShootResultDTO();
            var hit = Fields[positionY, positionX].Shoot();
            if (hit)
            {
                IShip shipHit = Ships.SingleOrDefault(x => x.TryToShoot(positionX, positionY));
                if (shipHit != null)
                {
                    result.IsShipHit = true;
                    result.IsShipSunk = shipHit.IsSunk;
                }
                else throw new GameLogicalException("Inconsistent fields on board and ships");
            }
            return result;
        }
    }
}
