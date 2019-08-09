using BattleShips.Core.Exceptions;
using System;
using System.Linq;

namespace BattleShips.Core.GameEntities.Structs
{
    public struct Coordinate
    {
        public int PositionX { get; }
        public int PositionY { get; }

        public Coordinate(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public static Coordinate? GetNumericalPosition(string boardPosition)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(boardPosition))
                    return null;

                char[] userCoordinate = boardPosition.ToCharArray();

                var letterPart = char.ToUpper(userCoordinate[0]);
                var numericalPartArray = userCoordinate.Skip(1).ToArray();
                var numericalPart = string.Join("", numericalPartArray);

                // Obtain position by moving char by 65 positions, so A becomes 0
                int posX = letterPart - 65;

                // Subtract 1 from numerical part to obtain zero-index array position
                int.TryParse(numericalPart, out int posY);
                posY--;

                if (posX >= 0 && posY >= 0)
                {
                    var coordinate = new Coordinate(posX, posY);
                    return coordinate;
                }
                else return null;
            }
            catch (Exception e)
            {
                throw new GameException("Numerical position could not be calculated", e);
            }
        }

        public static string GetBoardPosition(Coordinate coordinate)
        {
            if (coordinate.PositionX < 0 || coordinate.PositionY < 0)
                throw new GameArgumentException("Coordinate is negative");

            // Obtain letter by moving char by 65 positions, so 0 becomes A
            var letterPart = Convert.ToChar(coordinate.PositionX + 65);

            // Add 1 to numerical part to obtain one-index array position
            var numericalPart = coordinate.PositionY + 1;

            var result = letterPart.ToString() + numericalPart;
            return result;
        }
    }
}
