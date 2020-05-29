using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace DeMol2018.BitcoinGame.ReactApp
{
    public class Program
    {
        private static readonly IConfigurationRoot Configuration = ConfigurationFactory.Create();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(Configuration["Api:BaseUrl"])
                .Build();
    }
}
