using DeMol2018.BitcoinGame.Application.Services;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.GameServer.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeMol2018.BitcoinGame.GameServer
{
    public class Startup
    {
        private static readonly IConfigurationRoot Configuration = ConfigurationFactory.Create();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .WithOrigins(Configuration["Cors:Origins"]!.Split(','))
                            .AllowCredentials();
                    });
            });

            var connectionString = Configuration.GetConnectionString("BitcoinGameDatabase");
            services.AddDbContext<BitcoinGameDbContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<GameService>();
            services.AddTransient<PlayerService>();
            services.AddTransient<TransactionService>();
            services.AddTransient<WalletService>();

            services.AddSignalR();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BitcoinGameDbContext>();
                context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
