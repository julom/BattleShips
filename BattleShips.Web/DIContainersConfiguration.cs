using BattleShips.Core;
using BattleShips.Core.GameEntities.Factories;
using BattleShips.Core.GameEntities.Factories.Abstract;
using BattleShips.Core.GameEntities.Utils;
using BattleShips.Core.GameEntities.Utils.Abstract;
using BattleShips.Core.GameEntities.Validators;
using BattleShips.Core.GameEntities.Validators.Abstract;
using BattleShips.GameRepository;
using BattleShips.Web.Services;
using BattleShips.Web.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace BattleShips.Web
{
    public static class DIContainersConfiguration
    {
        public static void ConfigureDependencyInjectionContainers(this IServiceCollection services)
        {
            services.AddSingleton<IGameSettings, GameSettings>();
            services.AddSingleton<IGameRepository, InMemoryGameRepository>();

            services.AddScoped<IGameService, GameService>();

            services.AddTransient<IShipFactory, ShipFactory>();
            services.AddTransient<IBoardFactory, BoardFactory>();
            services.AddTransient<IShipPositionsRandomizer, ShipPositionsRandomizer>();
            services.AddTransient<IShipCoordinatesValidator, ShipCoordinatesValidator>();
            services.AddTransient<IShipVectorsValidator, ShipVectorsValidator>();
            services.AddTransient<IShipsGroupValidator, ShipsGroupValidator>();
        }
    }
}
