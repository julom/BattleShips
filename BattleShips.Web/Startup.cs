using BattleShips.Core;
using BattleShips.Core.GameEntities.Factories;
using BattleShips.Core.GameEntities.Factories.Abstract;
using BattleShips.Core.GameEntities.Validators;
using BattleShips.Core.GameEntities.Validators.Abstract;
using BattleShips.GameRepository;
using BattleShips.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BattleShips.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IGameSettings, GameSettings>();
            services.AddSingleton<IGameRepository, GameRepository.GameRepository>();
            services.AddSingleton<IGameService, GameService>();
            services.AddTransient<IShipFactory, ShipFactory>();
            services.AddTransient<IBoardFactory, BoardFactory>();
            services.AddTransient<IShipCoordinatesValidator, ShipCoordinatesValidator>();
            services.AddTransient<IShipVectorsValidator, ShipVectorsValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "gameInProgress",
                    template: "{controller=Game}/{action=Index}/{shootPositionX?}/{shootPositionY?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Game}/{action=Index}/{id?}");
            });
        }
    }
}
