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
        [Required]
        public IList<KeyValuePair<UserFieldCoordinate, UserFieldCoordinate>> ShipsUserCoordinates { get; set; }
        public bool[] ShipsFields { get; set; }

        
        public IList<KeyValuePair<ShipVector,ShipVector>> GetShipVectors()
        {
            var shipVectors = new List<KeyValuePair<ShipVector, ShipVector>>();
            if (ShipsUserCoordinates != null)
            {
                foreach(var shipCoordinate in ShipsUserCoordinates)
                {
                    try
                    {
                        var numericalCoordinateFrom = shipCoordinate.Key.GetNumericalCoordinates();
                        var numericalCoordinateTo = shipCoordinate.Value.GetNumericalCoordinates();
                        var shipVectorX = new ShipVector(numericalCoordinateFrom.PositionX, numericalCoordinateTo.PositionX);
                        var shipVectorY = new ShipVector(numericalCoordinateFrom.PositionY, numericalCoordinateTo.PositionY);
                        shipVectors.Add(new KeyValuePair<ShipVector, ShipVector>(shipVectorX, shipVectorY));
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
            //[StringLength(2, MinimumLength = 2, ErrorMessage = "Please specify two characters string, like A2")]
            //[RegularExpression("[a-j]|[A-J][0-9]", ErrorMessage = "Please specify two characters string, like A2")]
            //[Required]
            public string UserCoordinate { get; set; }

            public Coordinate GetNumericalCoordinates()
            {
                try
                {
                    char[] userCoordinate = UserCoordinate.ToCharArray();

                    var letterPart = char.ToUpper(userCoordinate[0]);
                    var numericalPartArray = userCoordinate.Skip(1).ToArray();
                    var numericalPart = string.Join("", numericalPartArray);

                    int posX = letterPart - 65;
                    int.TryParse(numericalPart, out int posY);
                    posY--;

                    var coordinate = new Coordinate(posX, posY);
                    return coordinate;
                }
                catch (GameArgumentException e)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
