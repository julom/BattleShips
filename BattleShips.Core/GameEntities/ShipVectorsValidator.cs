using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Structs;
using System.Linq;

namespace BattleShips.Core.GameEntities
{
    public class ShipVectorsValidator : IShipVectorsValidator
    {
        const int AbsoluteMinimumVectorSize = 2;

        public bool Validate(ShipVector vectorX, ShipVector vectorY)
        {
            if (ValidateVectorsSize(vectorX, vectorY))
            {
                return ValidateVectorsDiagonality(vectorX, vectorY);
            }

            return false;
        }

        private static bool ValidateVectorsSize(ShipVector vectorX, ShipVector vectorY)
        {
            if (vectorX.Size < AbsoluteMinimumVectorSize && vectorY.Size < AbsoluteMinimumVectorSize)
            {
                // at least one vector should have absolute minimum size
                return false;
            }
            if (vectorX.Size < GameSettings.ShipSizes.Min() && vectorY.Size < GameSettings.ShipSizes.Min())
            {
                // at least one vector should have game defined minimum size
                return false;
            }
            if (vectorX.Size > GameSettings.ShipSizes.Max() || vectorY.Size > GameSettings.ShipSizes.Max())
            {
                // neithers of vectors cannot have size bigger than game defined
                return false;
            }
            return true;
        }

        private static bool ValidateVectorsDiagonality(ShipVector vectorX, ShipVector vectorY)
        {
            return vectorX.Size == 1 || vectorY.Size == 1;
        }
    }
}