using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Factories;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.Core.GameEntities.Utils.Abstract;
using BattleShips.Core.GameEntities.Validators.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.GameEntities.Utils
{
    public class ShipPositionsRandomizer : IShipPositionsRandomizer
    {
        private readonly IGameSettings _gameSettings;
        private readonly IShipFactory _shipFactory;
        private readonly IShipsGroupValidator _shipsGroupValidator;

        public ShipPositionsRandomizer(IGameSettings gameSettings, IShipFactory shipFactory, IShipsGroupValidator shipsGroupValidator)
        {
            _gameSettings = gameSettings;
            _shipFactory = shipFactory;
            _shipsGroupValidator = shipsGroupValidator;
        }

        public IShip[] RandomizeShipsPositions()
        {
            var addedShipPositions = new List<IShip>();
            var random = new Random();

            var shipsLeft = new List<int>(_gameSettings.ShipSizes);
            while (shipsLeft.Any())
            {
                var currentShipSize = shipsLeft.Max();

                List<IShip> possiblePositions = GetPossibleShipPositions(addedShipPositions, currentShipSize);

                var chosenPossiblePosition = possiblePositions[random.Next(possiblePositions.Count)];
                addedShipPositions.Add(chosenPossiblePosition);

                shipsLeft.Remove(currentShipSize);
            }

            var resultShips = addedShipPositions.ToArray();

            _shipsGroupValidator.ValidateShips(resultShips);

            return resultShips;
        }

        private List<IShip> GetPossibleShipPositions(IList<IShip> addedShipPositions, int currentShipSize)
        {
            var possiblePositions = new List<IShip>();

            var vectorDifference = currentShipSize - 1; // i.e. vector (5,9) is for ship length of 5, but 9 - 5 = 4

            // add possible positions of vertically alligned ships
            for (int row = 0; row < _gameSettings.BoardSizeX - vectorDifference; row++)
            {
                for (int col = 0; col < _gameSettings.BoardSizeY; col++)
                {
                    var vectorVerticalLayout = new ShipLayout(new ShipVector(row, row + vectorDifference), new ShipVector(col, col));
                    if (!addedShipPositions.Any(x => VectorsOverlapsShip(x, vectorVerticalLayout)))
                    {
                        possiblePositions.Add(_shipFactory.Create(vectorVerticalLayout.VectorX, vectorVerticalLayout.VectorY));
                    }
                }
            }

            // add possible positions of horizontally alligned ships
            for (int row = 0; row < _gameSettings.BoardSizeX; row++)
            {
                for (int col = 0; col < _gameSettings.BoardSizeY - vectorDifference; col++)
                {
                    var vectorHorizontalLayout = new ShipLayout(new ShipVector(row, row), new ShipVector(col, col + vectorDifference));
                    if (!addedShipPositions.Any(x => VectorsOverlapsShip(x, vectorHorizontalLayout)))
                    {
                        possiblePositions.Add(_shipFactory.Create(vectorHorizontalLayout.VectorX, vectorHorizontalLayout.VectorY));
                    }
                }
            }

            return possiblePositions;
        }

        public bool VectorsOverlapsShip(IShip ship, ShipLayout vectors)
        {
            return
                ship.Coordinates.Any(x => vectors.Values.Any(val => val.Equals(x.Position)));
        }
    }
}
