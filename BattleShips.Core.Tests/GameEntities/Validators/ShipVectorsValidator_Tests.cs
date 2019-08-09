using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Structs;
using BattleShips.Core.GameEntities.Validators;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using BattleShips.Tests;

namespace BattleShips.Core.Tests.GameEntities
{
    [TestFixture]
    public class ShipVectorsValidator_Tests
    {
        private static IGameSettings _gameSettings;
        ShipVectorsValidator validator = new ShipVectorsValidator(_gameSettings);

        static ShipVectorsValidator_Tests()
        {
            var services = new ServiceCollection();
            services.AddScoped<IGameSettings, TestGameSettings>();
            var serviceProvider = services.BuildServiceProvider();

            _gameSettings = serviceProvider.GetService<IGameSettings>();
            _gameSettings.BoardSizeX = 4;
            _gameSettings.BoardSizeY = 4;
            _gameSettings.ShipSizes = new int[] {4};
        }

        public static IEnumerable<TestCaseData> ProperVectors()
        {
            yield return new TestCaseData(new ShipVector(0, 3), new ShipVector(1, 1));
            yield return new TestCaseData(new ShipVector(2, 2), new ShipVector(0, 3));
        }

        public static IEnumerable<TestCaseData> WrongVectorsExceedingBoard()
        {
            yield return new TestCaseData(new ShipVector(0, _gameSettings.BoardSizeX + 1), new ShipVector(1, 1));
            yield return new TestCaseData(new ShipVector(2, 2), new ShipVector(0, _gameSettings.BoardSizeY + 1));
        }

        public static IEnumerable<TestCaseData> DiagonalVectors()
        {
            yield return new TestCaseData(new ShipVector(0, 2), new ShipVector(1, 2));
            yield return new TestCaseData(new ShipVector(1, 2), new ShipVector(0, 2));
        }

        public static IEnumerable<TestCaseData> TooShortVectors()
        {
            List<TestCaseData> list = new List<TestCaseData>();
            for (int i = 0; i < _gameSettings.ShipSizes.Min() - 1; i++)
            {
                var testCase = new TestCaseData(new ShipVector(1, i), new ShipVector(1, 1));
                list.Add(testCase);
                yield return testCase;
            }
        }

        public static IEnumerable<TestCaseData> TooBigVectors()
        {
            List<TestCaseData> list = new List<TestCaseData>();
            for (int i = _gameSettings.ShipSizes.Max() + 1; i < _gameSettings.ShipSizes.Max() + 2; i++)
            {
                var testCase = new TestCaseData(new ShipVector(1, i), new ShipVector(1, 1));
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
            Assert.IsFalse(validator.Validate(vectorX, vectorY), $"Vectors smaller than {_gameSettings.ShipSizes.Min()} should not pass");
        }

        [Test]
        [TestCaseSource(nameof(TooBigVectors))]
        public void Ship_HasVectorMoreThanBiggestShipSize_ReturnsFalse(ShipVector vectorX, ShipVector vectorY)
        {
            Assert.IsFalse(validator.Validate(vectorX, vectorY), $"Vectors bigger than {_gameSettings.ShipSizes.Max()} should not pass");
        }
    }
}
