using BattleShips.Core;
using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BattleShips.Web.Models
{
    public class UserShipsLocationViewModel
    {
        private IList<KeyValuePair<ShipVector, ShipVector>> _shipVectors;

        public IList<KeyValuePair<UserFieldCoordinate, UserFieldCoordinate>> ShipsUserCoordinates { get; set; }

        public bool[] ShipsFields { get; set; }

        [MinLength(3, ErrorMessage = "Please specify exact number of ships = 3. ")]
        public IList<KeyValuePair<ShipVector, ShipVector>> ShipVectors
        {
            get => _shipVectors = _shipVectors ?? GetShipVectors();
        }


        private IList<KeyValuePair<ShipVector, ShipVector>> GetShipVectors()
        {
            var shipVectors = new List<KeyValuePair<ShipVector, ShipVector>>();
            if (ShipsUserCoordinates != null)
            {
                foreach (var shipCoordinate in ShipsUserCoordinates)
                {
                    try
                    {
                        var numericalCoordinateFrom = shipCoordinate.Key.NumericalCoordinate;
                        var numericalCoordinateTo = shipCoordinate.Value.NumericalCoordinate;
                        if (numericalCoordinateFrom.HasValue && numericalCoordinateTo.HasValue)
                        {
                            var shipVectorX = new ShipVector(numericalCoordinateFrom.Value.PositionX, numericalCoordinateTo.Value.PositionX);
                            var shipVectorY = new ShipVector(numericalCoordinateFrom.Value.PositionY, numericalCoordinateTo.Value.PositionY);
                            shipVectors.Add(new KeyValuePair<ShipVector, ShipVector>(shipVectorX, shipVectorY));
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return shipVectors;
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
                get => _numericalCoordinate = _numericalCoordinate ?? GetNumericalCoordinates();
            }

            private Coordinate? GetNumericalCoordinates()
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(UserCoordinate))
                        return null;

                    char[] userCoordinate = UserCoordinate.ToCharArray();

                    var letterPart = char.ToUpper(userCoordinate[0]);
                    var numericalPartArray = userCoordinate.Skip(1).ToArray();
                    var numericalPart = string.Join("", numericalPartArray);

                    int posX = letterPart - 65;
                    int.TryParse(numericalPart, out int posY);
                    posY--;

                    if (posX >= 0 && posY >= 0)
                    {
                        var coordinate = new Coordinate(posX, posY);
                        return coordinate;
                    }
                    else return null;
                }
                catch (GameArgumentException)
                {
                    return null;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
