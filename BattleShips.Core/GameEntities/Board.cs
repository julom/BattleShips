using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Enums;
using BattleShips.Core.GameEntities.Structs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.GameEntities
{
    public class Board : IBoard
    {
        public IShip[] Ships { get; private set; }

        public IField[,] Fields { get; private set; }

        public bool AreAllShipsSunk
        {
            get
            {
                return Ships.All(x => x.IsSunk);
            }
        }

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
            IField[,] outputFields = new Field[GameSettings.BoardSizeX, GameSettings.BoardSizeY];

            foreach (var ship in ships)
            {
                foreach (var shipSegment in ship.Coordinates)
                {
                    outputFields[shipSegment.PositionX, shipSegment.PositionY] = new Field(FieldTypes.Ship, shipSegment.PositionX, shipSegment.PositionY);
                }
            }
            
            for (int row = 0; row < GameSettings.BoardSizeX; row++)
            {
                for (int col = 0; col < GameSettings.BoardSizeY; col++)
                {
                    if (outputFields[row, col] == null)
                    {
                        outputFields[row, col] = new Field(FieldTypes.Empty, row, col);
                    }
                }
            }
            
            return outputFields;
        }

        public bool VectorsOverlapsShip(IShip ship, VectorLayout vectors)
        {
            return
                ship.Coordinates.Any(x => vectors.Values.Any(val => val.Equals(x.Position)));
        }

        public void RandomizeShipsPositions(IList<int> shipSizes)
        {
            var existingShipPositions = new List<IShip>();
            var random = new Random();

            var shipsLeft = new List<int>(GameSettings.ShipSizes);
            while (shipsLeft.Any())
            {
                var currentShipSize = shipsLeft.Max();
                var vectorDifference = currentShipSize - 1; // i.e. vector (5,9) is for ship length of 5, but 9 - 5 = 4

                var possiblePositions = new List<IShip>();

                // add possible positions of vertically alligned ships
                for (int row = 0; row < GameSettings.BoardSizeX - vectorDifference; row++)
                {
                    for (int col = 0; col < GameSettings.BoardSizeY; col++)
                    {
                        var vectorVerticalLayout = new VectorLayout(new ShipVector(row, row + vectorDifference), new ShipVector(col, col));
                        if (!existingShipPositions.Any(x => VectorsOverlapsShip(x, vectorVerticalLayout)))
                        {
                            possiblePositions.Add(new Ship(vectorVerticalLayout.VectorX, vectorVerticalLayout.VectorY));
                        }
                    }
                }

                // add possible positions of horizontally alligned ships
                for (int row = 0; row < GameSettings.BoardSizeX; row++)
                {
                    for (int col = 0; col < GameSettings.BoardSizeY - vectorDifference; col++)
                    {
                        var vectorHorizontalLayout = new VectorLayout(new ShipVector(row, row), new ShipVector(col, col + vectorDifference));
                        if (!existingShipPositions.Any(x => VectorsOverlapsShip(x, vectorHorizontalLayout)))
                        {
                            possiblePositions.Add(new Ship(vectorHorizontalLayout.VectorX, vectorHorizontalLayout.VectorY));
                        }
                    }
                }

                var chosenPossiblePosition = possiblePositions[random.Next(possiblePositions.Count)];
                existingShipPositions.Add(chosenPossiblePosition);

                shipsLeft.Remove(currentShipSize);
            }

            Ships = existingShipPositions.ToArray();
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

            if (shipsFields.Count != GameSettings.ShipSizes.Count)
            {
                throw new GameArgumentException("Ship count not matched with game settings");
            }

            if (fields.Length != GameSettings.ShipSizes.Sum())
            {
                throw new GameArgumentException("Ship fields count not matched with game settings");
            }

            foreach (var shipFields in shipsFields)
            {
                var ship = new Ship(shipFields);
                ships.Add(ship);
            }            

            return ships.ToArray();
        }

        private bool CheckIfFieldBelongsToShip(KeyValuePair<int, int> field, IList<KeyValuePair<int, int>> shipFields)
        {
            if (shipFields == null) return false;

            if (shipFields.Any(x => x.Key + 1 == field.Key && x.Value == field.Value || 
                                    x.Value + 1 == field.Value && x.Key == field.Key))
            {
                return true;
            }
            return false;
        }

        public ShootResultDTO Shoot(int positionX, int positionY)
        {
            ShootResultDTO result = new ShootResultDTO();
            var hit = Fields[positionX, positionY].Shoot();
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
