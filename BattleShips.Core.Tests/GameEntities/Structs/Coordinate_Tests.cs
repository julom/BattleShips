using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities.Structs;
using NUnit.Framework;
using System;

namespace BattleShips.Core.Tests.GameEntities.Structs
{
    [TestFixture]
    public class Coordinate_Tests
    {
        [Test, Pairwise]
        public void GetNumericalPosition_HappyPath([Values("a", "A", "z", "Z")] string letter, [Range(1, 30)] int number)
        {
            var boardPosition = letter + number;

            var result = Coordinate.GetNumericalPosition(boardPosition);

            Assert.IsInstanceOf<Coordinate>(result);
            Assert.IsNotNull(result);
        }

        [Test, Pairwise]
        public void GetNumericalPosition_WrongNumber_ReturnsNull([Values("a", "A", "z", "Z")] string letter, [Values(-1,0)] int number)
        {
            var boardPosition = letter + number;

            var result = Coordinate.GetNumericalPosition(boardPosition);

            Assert.IsNull(result, "Coordinate should be null when number part is smaller than 1");
        }

        [Test, Pairwise]
        public void GetNumericalPosition_WrongLetter_ReturnsNull([Values("1", ";", ",", "-", "@")] string letter, [Range(1, 10)] int number)
        {
            var boardPosition = letter + number;

            var result = Coordinate.GetNumericalPosition(boardPosition);

            Assert.IsNull(result, "Coordinate should be null when letter part is other sign then letter");
        }

        [Test, Pairwise]
        public void GetNumericalPosition_InputNull_ReturnsNull()
        {
            string boardPosition = null;

            var result = Coordinate.GetNumericalPosition(boardPosition);

            Assert.IsNull(result, "Coordinate should be null when input is null");
        }

        [Test, Pairwise]
        public void GetBoardPosition_HappyPath([Range(0, 20)] int positionX, [Range(0, 20)] int positionY)
        {
            var numericalPosition = new Coordinate(positionX, positionY);

            var result = Coordinate.GetBoardPosition(numericalPosition);

            Assert.IsInstanceOf<string>(result);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Sequential]
        public void GetBoardPosition_EitherPositionIsNegative_([Values(-1, 0)] int positionX, [Values(0, -1)] int positionY)
        {
            var numericalPosition = new Coordinate(positionX, positionY);

            Action action = () => Coordinate.GetBoardPosition(numericalPosition);

            Assert.Throws<GameArgumentException>(() => action());
        }
    }
}
