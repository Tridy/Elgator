using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Elgator
{
    public class Configuration
    {
        public string Url { get; set; }

        public int MinBrightness { get; set; }
        public int MaxBrightness { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }

        public int UpdateInMilliseconds { get; set; }

        public static IEnumerable<Configuration> FromConfiguration(IConfiguration configuration)
        {
            var configurations = new List<Configuration>();

            int i = 0;

            while(configuration[$"settings:{i}:Url"] != null)
            {
                var c = new Configuration
                {
                    Url = configuration[$"settings:{i}:Url"],
                    MinBrightness = int.Parse(configuration[$"settings:{i}:MinBrightness"]),
                    MaxBrightness = int.Parse(configuration[$"settings:{i}:MaxBrightness"]),
                    MinTemperature = int.Parse(configuration[$"settings:{i}:MinTemperature"]),
                    MaxTemperature = int.Parse(configuration[$"settings:{i}:MaxTemperature"]),
                    UpdateInMilliseconds = int.Parse(configuration[$"settings:{i}:UpdateInMilliseconds"])
                };

                i++;

                if(i > 100)
                {
                    throw new IndexOutOfRangeException("Too many settings");
                }

                configurations.Add(c);
            }

            return configurations;
        }
    }
}