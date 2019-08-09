using BattleShips.Core.GameEntities.Abstract;

namespace BattleShips.Core.GameEntities.Validators.Abstract
{
    public interface IShipsGroupValidator
    {
        void ValidateShips(IShip[] ships);
    }
}
