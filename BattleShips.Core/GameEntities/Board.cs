using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Enums;
using System.Linq;
using BattleShips.Core.GameEntities.Utils.Abstract;
using BattleShips.Core.GameEntities.Validators.Abstract;

namespace BattleShips.Core.GameEntities
{
    public class Board : IBoard
    {
        private readonly IGameSettings _gameSettings;
        private readonly IShipPositionsRandomizer _shipPositionsRandomizer;
        private readonly IShipsGroupValidator _shipsGroupValidator;

        public IShip[] Ships { get; private set; }
        public IField[,] Fields { get; private set; }
        public bool AreAllShipsSunk
        {
            get
            {
                return Ships.All(x => x.IsSunk);
            }
        }

        public Board(IShip[] ships, IShipsGroupValidator shipsGroupValidator, IGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _shipsGroupValidator = shipsGroupValidator;
            _shipsGroupValidator.ValidateShips(ships);
            Ships = ships;
            Fields = FillTheFields(Ships);
        }

        public Board(IShipPositionsRandomizer shipPositionsRandomizer, IGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _shipPositionsRandomizer = shipPositionsRandomizer;
            Ships = _shipPositionsRandomizer.RandomizeShipsPositions();
            Fields = FillTheFields(Ships);
        }

        private IField[,] FillTheFields(IShip[] ships)
        {
            IField[,] outputFields = new Field[_gameSettings.BoardSizeX, _gameSettings.BoardSizeY];

            foreach (var ship in ships)
            {
                foreach (var shipSegment in ship.Coordinates)
                {
                    outputFields[shipSegment.PositionX, shipSegment.PositionY] = new Field(FieldTypes.Ship, shipSegment.PositionX, shipSegment.PositionY);
                }
            }
            
            for (int row = 0; row < _gameSettings.BoardSizeX; row++)
            {
                for (int col = 0; col < _gameSettings.BoardSizeY; col++)
                {
                    if (outputFields[row, col] == null)
                    {
                        outputFields[row, col] = new Field(FieldTypes.Empty, row, col);
                    }
                }
            }
            
            return outputFields;
        }
        
        public ShootResultDTO Shoot(int positionX, int positionY)
        {
            ShootResultDTO result = new ShootResultDTO() { PositionX = positionX, PositionY = positionY};
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
