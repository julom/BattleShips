using BattleShips.Core.Exceptions;
using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Enums;
using BattleShips.Core.GameEntities.Validators;
using BattleShips.Tests;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Core.Tests.GameEntities.Validators
{
    [TestFixture]
    public class ShipsGroupValidator_Tests
    {
        private readonly IGameSettings _gameSettings;
        ShipsGroupValidator shipsGroupValidator;
        Mock<IShip> mockShip1;
        Mock<IShip> mockShip2;

        public ShipsGroupValidator_Tests()
        {
            IServiceProvider serviceProvider = DIContainersTestConfiguration.GetDIServiceProvider();
            _gameSettings = serviceProvider.GetService<IGameSettings>();
        }

        [SetUp]
        public void Init()
        {
            shipsGroupValidator = new ShipsGroupValidator(_gameSettings);
            _gameSettings.ShipSizes = new List<int> { 2, 2};
            mockShip1 = new Mock<IShip>();
            mockShip2 = new Mock<IShip>();
            mockShip1.Setup(x => x.Size).Returns(2);
            mockShip2.Setup(x => x.Size).Returns(2);
        }

        [Test]
        public void ValidateShips_HappyPath()
        {
            var ship1Fields = new List<IField> { new Field(FieldTypes.Ship, 0, 1), new Field(FieldTypes.Ship, 0, 2) };
            var ship2Fields = new List<IField> { new Field(FieldTypes.Ship, 1, 1), new Field(FieldTypes.Ship, 1, 2) };
            mockShip1.Setup(x => x.Coordinates).Returns(ship1Fields);
            mockShip2.Setup(x => x.Coordinates).Returns(ship2Fields);
            var ships = new IShip[]{ mockShip1.Object, mockShip2.Object };

            Action action = () => shipsGroupValidator.ValidateShips(ships);

            Assert.DoesNotThrow(() => action());
        }

        [Test]
        public void ValidateShips_NumberOfShipsDifferentThanInGameOptions_ThrowsGameArgumentException([Values(0, 1, 3)] int numberOfShips)
        {
            var ship1Fields = new List<IField> { new Field(FieldTypes.Ship, 0, 1), new Field(FieldTypes.Ship, 0, 2) };
            var ship2Fields = new List<IField> { new Field(FieldTypes.Ship, 1, 1), new Field(FieldTypes.Ship, 1, 2) };
            mockShip1.Setup(x => x.Coordinates).Returns(ship1Fields);
            mockShip2.Setup(x => x.Coordinates).Returns(ship2Fields);
            var ships = new IShip[] { mockShip1.Object, mockShip2.Object, mockShip2.Object };
            ships = ships.Take(numberOfShips).ToArray();

            Action action = () => shipsGroupValidator.ValidateShips(ships);

            Assert.Throws<GameArgumentException>(() => action());
        }

        [Test]
        public void ValidateShips_NumberOfShipsSegmentsDifferentThanInGameOptions_ThrowsGameArgumentException([Values(0, 1, 3)] int numberOfShipSegmentsInEveryShip)
        {
            var ship1Fields = new List<IField> { new Field(FieldTypes.Ship, 0, 1), new Field(FieldTypes.Ship, 0, 2) };
            var ship2Fields = new List<IField> { new Field(FieldTypes.Ship, 1, 1), new Field(FieldTypes.Ship, 1, 2) };
            mockShip1.Setup(x => x.Coordinates).Returns(ship1Fields);
            mockShip2.Setup(x => x.Coordinates).Returns(ship2Fields);
            mockShip1.Setup(x => x.Size).Returns(numberOfShipSegmentsInEveryShip);
            mockShip2.Setup(x => x.Size).Returns(numberOfShipSegmentsInEveryShip);
            var ships = new IShip[] { mockShip1.Object, mockShip2.Object };

            Action action = () => shipsGroupValidator.ValidateShips(ships);

            Assert.Throws<GameArgumentException>(() => action());
        }

        [Test]
        public void ValidateShips_ShipsIntersect_ThrowsGameArgumentException()
        {
            var ship1Fields = new List<IField> { new Field(FieldTypes.Ship, 0, 1), new Field(FieldTypes.Ship, 0, 2) };
            var ship2Fields = new List<IField> { new Field(FieldTypes.Ship, 0, 1), new Field(FieldTypes.Ship, 1, 1) };
            mockShip1.Setup(x => x.Coordinates).Returns(ship1Fields);
            mockShip2.Setup(x => x.Coordinates).Returns(ship2Fields);
            var ships = new IShip[] { mockShip1.Object, mockShip2.Object };

            Action action = () => shipsGroupValidator.ValidateShips(ships);

            Assert.Throws<GameArgumentException>(() => action());
        }
    }
}
