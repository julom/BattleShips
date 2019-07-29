using System.Collections.Generic;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.Core.GameEntities.Validators.Abstract;

namespace BattleShips.Core.GameEntities.Factories
{
    public class ShipFactory : IShipFactory
    {
        private readonly IShipCoordinatesValidator _shipCoordinatesValidator;
        private readonly IShipVectorsValidator _shipVectorsValidator;

        public ShipFactory(IShipCoordinatesValidator shipCoordinatesValidator, IShipVectorsValidator shipVectorsValidator)
        {
            _shipCoordinatesValidator = shipCoordinatesValidator;
            _shipVectorsValidator = shipVectorsValidator;
        }

        public IShip Create(ShipVector vectorX, ShipVector vectorY)
        {
            return new Ship(vectorX, vectorY, _shipVectorsValidator);
        }

        public IShip Create(IList<KeyValuePair<int, int>> coordinates)
        {
            return new Ship(coordinates, _shipCoordinatesValidator);
        }
    }
}
