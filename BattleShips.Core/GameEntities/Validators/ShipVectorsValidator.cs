﻿using BattleShips.Core.GameEntities.Structs;
using BattleShips.Core.GameEntities.Validators.Abstract;
using System.Linq;

namespace BattleShips.Core.GameEntities.Validators
{
    public class ShipVectorsValidator : IShipVectorsValidator
    {
        const int AbsoluteMinimumVectorSize = 2;

        private readonly IGameSettings _gameSettings;

        public ShipVectorsValidator(IGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }


        public bool Validate(ShipVector vectorX, ShipVector vectorY)
        {
            if (ValidateVectorsSize(vectorX, vectorY))
            {
                return ValidateVectorsDiagonality(vectorX, vectorY);
            }

            return false;
        }

        private bool ValidateVectorsSize(ShipVector vectorX, ShipVector vectorY)
        {
            if (vectorX.Size < AbsoluteMinimumVectorSize && vectorY.Size < AbsoluteMinimumVectorSize)
            {
                // at least one vector should have absolute minimum size
                return false;
            }
            if (vectorX.Size < _gameSettings.ShipSizes.Min() && vectorY.Size < _gameSettings.ShipSizes.Min())
            {
                // at least one vector should have game defined minimum size
                return false;
            }
            if (vectorX.Size > _gameSettings.ShipSizes.Max() || vectorY.Size > _gameSettings.ShipSizes.Max())
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