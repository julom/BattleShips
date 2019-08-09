using BattleShips.Core.GameEntities.Structs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.Tests.GameEntities.Structs
{
    [TestFixture]
    public class ShipVector_Tests
    {
        [Test, Sequential]
        public void ShipVector_HappyPath([Values(0, 1, 2, 3)] int from, [Values(0, 3, 5, 10)] int to)
        {
            var shipVector = new ShipVector(from, to);

            Assert.NotNull(shipVector);
            Assert.AreEqual(from, shipVector.From);
            Assert.AreEqual(to, shipVector.To);
        }

        [Test, Sequential]
        public void ShipVector_ValuesInReverseOrder_RevertsBack([Values(0, 3, 5, 10)] int from, [Values(0, 1, 2, 3)] int to)
        {
            var smallerValue = Math.Min(from, to);
            var biggerValue = Math.Max(from, to);

            var shipVector = new ShipVector(from, to);

            Assert.AreEqual(smallerValue, shipVector.From);
            Assert.AreEqual(biggerValue, shipVector.To);
        }

        [Test, Sequential]
        public void Size_ReturnsCountOfValuesInside([Values(0, 1, 2, 3)] int from, [Values(0, 3, 5, 10)] int to)
        {
            var size = to - from + 1;

            var shipVector = new ShipVector(from, to);

            Assert.AreEqual(size, shipVector.Size);
        }

        [Test, Sequential]
        public void Values_ReturnsValuesInside([Values(0, 1, 2, 3)] int from, [Values(0, 3, 5, 10)] int to)
        {
            var values = new List<int>();
            for (int i = from; i <= to; i++)
            {
                values.Add(i);
            }

            var shipVector = new ShipVector(from, to);

            Assert.AreEqual(values, shipVector.Values.ToList());
        }
    }
}
