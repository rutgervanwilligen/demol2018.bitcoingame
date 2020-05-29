using DeMol2018.BitcoinGame.Application.Services;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.ReactApp.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeMol2018.BitcoinGame.ReactApp
{
    public class Startup
    {
        private static readonly IConfigurationRoot Configuration = ConfigurationFactory.Create();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("BitcoinGameDatabase");
            services.AddDbContext<BitcoinGameDbContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<GameService>();
            services.AddTransient<PlayerService>();
            services.AddTransient<TransactionService>();
            services.AddTransient<WalletService>();

            services.AddSignalR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();

            app.UseEndpoints(routes =>
            {
                routes.MapHub<BitcoinGameHub>("/bitcoinGameHub");

                routes.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                routes.MapFallbackToController("Index", "Home");
            });

            app.UseStaticFiles();
        }
    }
}
