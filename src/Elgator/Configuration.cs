using Microsoft.Extensions.Configuration;

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

        public static Configuration FromConfiguration(IConfiguration configuration)
        {
            var c = new Configuration
            {
                Url = configuration["Url"],
                MinBrightness = int.Parse(configuration["MinBrightness"]),
                MaxBrightness = int.Parse(configuration["MaxBrightness"]),
                MinTemperature = int.Parse(configuration["MinTemperature"]),
                MaxTemperature = int.Parse(configuration["MaxTemperature"]),
                UpdateInMilliseconds = int.Parse(configuration["UpdateInMilliseconds"]),
            };

            return c;
        }
    }
}