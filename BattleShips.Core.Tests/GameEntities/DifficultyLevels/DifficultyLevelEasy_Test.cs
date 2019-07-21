using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.DifficultyLevels;
using BattleShips.Core.GameEntities.Enums;
using Moq;
using NUnit.Framework;

namespace BattleShips.Core.Tests.GameEntities.DifficultyLevels
{
    [TestFixture]
    public class DifficultyLevelEasy_Test
    {
        DifficultyLevelEasy difficulty;
        Mock<IBoard> mockBoard = new Mock<IBoard>();

        [SetUp]
        public void Init()
        {
            mockBoard.Setup(x => x.Fields).Returns(new Field[,] { { new Field(FieldTypes.Empty, 0, 0) } });
        }

        [Test]
        public void ChooseShotCoordinates_ReturnsCoordinatesWithinBoard()
        {
            difficulty = new DifficultyLevelEasy();

            var result = difficulty.ChooseShotCoordinates(mockBoard.Object);

            Assert.LessOrEqual(result.Key, mockBoard.Object.Fields.Length - 1);
            Assert.LessOrEqual(result.Value, mockBoard.Object.Fields.Length - 1);
        }
    }
}
