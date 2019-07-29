using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Structs;

namespace BattleShips.Core.GameEntities.Utils.Abstract
{
    public interface IShipPositionsRandomizer
    {
        IShip[] RandomizeShipsPositions();
        bool VectorsOverlapsShip(IShip ship, VectorLayout vectors);
    }
}
