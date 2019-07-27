using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Structs;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class ShipVectorsValidator_Tests
    {
        ShipVectorsValidator validator = new ShipVectorsValidator();


        public static IEnumerable<TestCaseData> ProperVectors()
        {
            yield return new TestCaseData(new ShipVector(0, 2), new ShipVector(1, 1));
            yield return new TestCaseData(new ShipVector(2, 2), new ShipVector(0, 2));
        }

        public static IEnumerable<TestCaseData> WrongVectorsExceedingBoard()
        {
            yield return new TestCaseData(new ShipVector(0, GameSettings.BoardSizeX + 1), new ShipVector(1, 1));
            yield return new TestCaseData(new ShipVector(2, 2), new ShipVector(0, GameSettings.BoardSizeY + 1));
        }

        public static IEnumerable<TestCaseData> DiagonalVectors()
        {
            yield return new TestCaseData(new ShipVector(0, 2), new ShipVector(1, 2));
            yield return new TestCaseData(new ShipVector(1, 2), new ShipVector(0, 2));
        }

        public static IEnumerable<TestCaseData> TooShortVectors()
        {
            List<TestCaseData> list = new List<TestCaseData>();
            for (int i = 0; i < GameSettings.ShipSizes.Min() - 1; i++)
            {
                var testCase = new TestCaseData(new ShipVector(0, i), new ShipVector(1, i));
                list.Add(testCase);
                yield return testCase;
            }
        }

        public static IEnumerable<TestCaseData> TooBigVectors()
        {
            List<TestCaseData> list = new List<TestCaseData>();
            for (int i = 0; i < GameSettings.ShipSizes.Max() + 1; i++)
            {
                var testCase = new TestCaseData(new ShipVector(0, i), new ShipVector(1, i));
                list.Add(testCase);
                yield return testCase;
            }
        }

        [Test]
        [TestCaseSource(nameof(ProperVectors))]
        public void Ship_HasProperVectors_ReturnsTrue(ShipVector vectorX, ShipVector vectorY)
        {
            Assert.IsTrue(validator.Validate(vectorX, vectorY), "Proper vectors not passed");
        }

        [Test]
        [TestCaseSource(nameof(WrongVectorsExceedingBoard))]
        public void Ship_HasVectorsExceedingBoard_ReturnsFalse(ShipVector vectorX, ShipVector vectorY)
        {
            Assert.IsFalse(validator.Validate(vectorX, vectorY), "Vectors exceeding board size should not pass");
        }

        [Test]
        [TestCaseSource(nameof(DiagonalVectors))]
        public void Ship_HasDiagonalVectors_ReturnsFalse(ShipVector vectorX, ShipVector vectorY)
        {
            Assert.IsFalse(validator.Validate(vectorX, vectorY), "Diagonal vectors should not pass");
        }

        [Test]
        [TestCaseSource(nameof(TooShortVectors))]
        public void Ship_HasVectorLessThanSmallestShipSize_ReturnsFalse(ShipVector vectorX, ShipVector vectorY)
        {
            Assert.IsFalse(validator.Validate(vectorX, vectorY), $"Vectors smaller than {GameSettings.ShipSizes.Min()} should not pass");
        }

        [Test]
        [TestCaseSource(nameof(TooBigVectors))]
        public void Ship_HasVectorMoreThanBiggestShipSize_ReturnsFalse(ShipVector vectorX, ShipVector vectorY)
        {
            Assert.IsFalse(validator.Validate(vectorX, vectorY), $"Vectors bigger than {GameSettings.ShipSizes.Max()} should not pass");
        }
    }
}
