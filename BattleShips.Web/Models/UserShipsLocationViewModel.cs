using BattleShips.Core;
using BattleShips.Core.GameEntities.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BattleShips.Web.Models
{
    public class UserShipsLocationViewModel
    {
        private IList<ShipLayout> _shipsLayouts;

        public IList<KeyValuePair<UserFieldCoordinate, UserFieldCoordinate>> ShipsUserCoordinates { get; set; }

        public bool[] ShipsFields { get; set; }

        [MinLength(GameSettings.ShipsCountDefault, ErrorMessage = "Please specify correct number of ships")]
        public IList<ShipLayout> ShipsLayouts
        {
            get
            {
                if (_shipsLayouts == null)
                    _shipsLayouts = GetShipLayouts(ShipsUserCoordinates);
                return _shipsLayouts;
            }
        }


        private static IList<ShipLayout> GetShipLayouts(IList<KeyValuePair<UserFieldCoordinate, UserFieldCoordinate>> shipsUserCoordinates)
        {
            var shipLayouts = new List<ShipLayout>();
            if (shipsUserCoordinates != null)
            {
                foreach (var shipCoordinate in shipsUserCoordinates)
                {
                    try
                    {
                        var numericalCoordinateFrom = shipCoordinate.Key.NumericalCoordinate;
                        var numericalCoordinateTo = shipCoordinate.Value.NumericalCoordinate;
                        if (numericalCoordinateFrom.HasValue && numericalCoordinateTo.HasValue)
                        {
                            var shipVectorX = new ShipVector(numericalCoordinateFrom.Value.PositionX, numericalCoordinateTo.Value.PositionX);
                            var shipVectorY = new ShipVector(numericalCoordinateFrom.Value.PositionY, numericalCoordinateTo.Value.PositionY);
                            shipLayouts.Add(new ShipLayout(shipVectorX, shipVectorY));
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return shipLayouts;
        }


        public class UserFieldCoordinate
        {
            private Coordinate? _numericalCoordinate;

            [StringLength(3, MinimumLength = 2, ErrorMessage = "Please specify two or three characters string. ")]
            [RegularExpression(@"([a-j]|[A-J])(10|[1-9])", ErrorMessage = "Please specify two or three characters string, like A2. ")]
            [Required(ErrorMessage = "This field is required. ")]
            public string UserCoordinate { get; set; }

            public Coordinate? NumericalCoordinate
            {
                get
                {
                    if (_numericalCoordinate == null)
                        _numericalCoordinate = Coordinate.GetNumericalPosition(UserCoordinate);
                    return _numericalCoordinate;
                }
            }
        }
    }
}
