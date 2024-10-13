using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DeMol2018.BitcoinGame.GameServer
{
    public static class ConfigurationFactory
    {
        public static IConfigurationRoot Create()
        {
            var basePath =
                Environment.GetEnvironmentVariable($"CONFIG_PATH") ??
                Directory.GetCurrentDirectory();

            return new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(basePath))
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        }
    }
}