using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Validators.Abstract;
using System.Linq;

namespace BattleShips.Core.GameEntities.Validators
{
    public class ShipsGroupValidator : IShipsGroupValidator
    {
        private readonly IGameSettings _gameSettings;

        public ShipsGroupValidator(IGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public void ValidateShips(IShip[] ships)
        {
            if (ships.Length != _gameSettings.ShipSizes.Count)
            {
                throw new GameArgumentException("Incorrect number of ships. Please correct ships input");
            }

            if (ships.Select(x => x.Size).Sum() != _gameSettings.ShipSizes.Sum())
            {
                throw new GameArgumentException("Incorrect number of ships segments. Please correct ships input");
            }

            var shipPositions = ships.SelectMany(x => x.Coordinates.Select(c => c.Position));
            if (shipPositions.Distinct().Count() != shipPositions.Count())
            {
                throw new GameArgumentException("Ships cannot intersect. Please correct ships input");
            }
        }
    }
}
