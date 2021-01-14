using conferma.core;
using conferma.infrastructure;
using Microsoft.Extensions.Configuration;
using System;

namespace conferma
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            var engine = new GameEngine(configuration, new ScoreRepository(configuration));
            engine.Run();
        }
    }
}
