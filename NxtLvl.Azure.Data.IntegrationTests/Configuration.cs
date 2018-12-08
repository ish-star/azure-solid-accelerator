using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    public static class Configuration
    {
        public static Dictionary<string, string> GetComsosConfig()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                   .AddJsonFile("appsettings.json", false, true)
                                                   .Build();

            return config.GetSection("cosmosDbSettings")
                         .GetChildren()
                         .ToDictionary(c => c.Key, c => c.Value);
        }
    }
}
