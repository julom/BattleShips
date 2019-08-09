using BattleShips.Core.GameEntities.Factories;
using BattleShips.Core.GameEntities.Utils;
using BattleShips.Core.GameEntities.Validators.Abstract;
using BattleShips.Tests;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;

namespace BattleShips.Core.Tests.GameEntities.Utils
{
    [TestFixture]
    public class ShipPositionsRandomizer_Tests
    {
        private readonly IGameSettings _gameSettings;
        private readonly IShipFactory _shipFactory;
        private readonly IShipsGroupValidator _shipsGroupValidator;
        ShipPositionsRandomizer shipPositionsRandomizer;

        public ShipPositionsRandomizer_Tests()
        {
            IServiceProvider serviceProvider = DIContainersTestConfiguration.GetDIServiceProvider();
            _gameSettings = serviceProvider.GetService<IGameSettings>();
            _shipFactory = serviceProvider.GetService<IShipFactory>();
            _shipsGroupValidator = serviceProvider.GetService<IShipsGroupValidator>();
        }

        [Test]
        [Repeat(100)]
        public void RandomizeShipsPositions_ReturnsShipsCountEqualsToGameSettings()
        {
            shipPositionsRandomizer = new ShipPositionsRandomizer(_gameSettings, _shipFactory, _shipsGroupValidator);

            var ships = shipPositionsRandomizer.RandomizeShipsPositions();

            Assert.AreEqual(_gameSettings.ShipSizes.Count, ships.Count());
        }

        [Test]
        [Repeat(100)]
        public void RandomizeShipsPositions_ReturnsShipsFieldsCountEqualsToGameSettings()
        {
            shipPositionsRandomizer = new ShipPositionsRandomizer(_gameSettings, _shipFactory, _shipsGroupValidator);

            var ships = shipPositionsRandomizer.RandomizeShipsPositions();

            Assert.AreEqual(_gameSettings.ShipSizes.Sum(), ships.Select(x => x.Size).Sum());
        }
    }
}
