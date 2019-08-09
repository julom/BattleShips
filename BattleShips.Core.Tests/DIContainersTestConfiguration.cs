using BattleShips.Core;
using BattleShips.Core.GameEntities.Factories;
using BattleShips.Core.GameEntities.Utils;
using BattleShips.Core.GameEntities.Utils.Abstract;
using BattleShips.Core.GameEntities.Validators;
using BattleShips.Core.GameEntities.Validators.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BattleShips.Tests
{
    static class DIContainersTestConfiguration
    {
        public static IServiceProvider GetDIServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddScoped<IGameSettings, TestGameSettings>();
            services.AddTransient<IShipFactory, ShipFactory>();
            services.AddTransient<IShipCoordinatesValidator, ShipCoordinatesValidator>();
            services.AddTransient<IShipVectorsValidator, ShipVectorsValidator>();
            services.AddTransient<IShipPositionsRandomizer, ShipPositionsRandomizer>();
            services.AddTransient<IShipsGroupValidator, ShipsGroupValidator>();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
