using System.Collections.Generic;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Structs;

namespace BattleShips.Core.GameEntities.Factories
{
    public interface IShipFactory
    {
        IShip Create(ShipVector vectorX, ShipVector vectorY);
        IShip Create(IList<KeyValuePair<int, int>> coordinates);
    }
}