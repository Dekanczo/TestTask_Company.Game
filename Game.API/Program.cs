using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Infrastructure.DAL;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Game.API
{
    public class Program
    {
        public static readonly ILoggerFactory MainLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var context = new ApplicationContext())
            {
                
                context.Database.Migrate();
                Seeder.Start();

                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
