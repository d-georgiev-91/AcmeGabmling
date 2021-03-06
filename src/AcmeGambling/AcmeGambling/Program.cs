﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using AcmeGambling.Services;
using AcmeGambling.Settings;

namespace AcmeGambling
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            // create service collection
            var services = new ServiceCollection();
            ConfigureServices(services);

            // create service provider
            var serviceProvider = services.BuildServiceProvider();

            // entry to run app
            await serviceProvider.GetService<ConsoleSlotMachine>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            services.Configure<SymbolsSettings>(configuration.GetSection("Symbols"));
            services.Configure<SlotMachineSettings>(configuration.GetSection("SlotMachine"));

            // add services:
            services.AddTransient<IRandomSymbolGenerator, RandomSymbolGenerator>();
            services.AddTransient<ISymbolsProvider, SymbolsProvider>();
            services.AddTransient<ISlotMachine, SlotMachine>();

            // add app
            services.AddTransient<ConsoleSlotMachine>();
        }
    }
}
