using Elgator;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ElgatoCommands
{
    internal class Program
    {
        private static Elgator.Elgator _elgator;

        private static Configuration _configuration;

        private static async Task Main(string[] args)
        {
            _configuration = GetConfiguration();

            _elgator = Elgator.Elgator.FromConfiguration(_configuration);

            Elgator.AccessoryInfo info = await _elgator.GetAccessoryInfo().ConfigureAwait(false);

            Console.WriteLine("==========================================");
            Console.WriteLine($"{nameof(info.Product)}: {info.Product}");
            Console.WriteLine($"{nameof(info.Name)}: {info.Name}");
            Console.WriteLine($"{nameof(info.SerialNumber)}: {info.SerialNumber}");
            Console.WriteLine($"{nameof(info.FirmwareBuild)}: {info.FirmwareBuild}");
            Console.WriteLine($"{nameof(info.FirmwareVersion)}: {info.FirmwareVersion}");
            Console.WriteLine($"{nameof(info.BoardType)}: {info.BoardType}");
            Console.WriteLine($"{nameof(info.Features)}: { string.Join(',', info.Features)}");
            Console.WriteLine("==========================================");

            await TurnOn().ConfigureAwait(false);

            await Task.Delay(1000).ConfigureAwait(false);

            await LoopBrightness().ConfigureAwait(false);

            await Task.Delay(1000).ConfigureAwait(false);

            await LoopTemperature().ConfigureAwait(false);

            await TurnOff().ConfigureAwait(false);

            Console.WriteLine("Any key to exit.");

            Console.ReadKey();
        }

        private static Configuration GetConfiguration()
        {
            var conf = new ConfigurationBuilder()
                .AddJsonFile("demo.config.json")
                .AddJsonFile("demo.config.local.json", optional: true)
                .Build();

            var configurations = Configuration.FromConfiguration(conf);

            return configurations.ElementAt(0);
        }

        private static async Task LoopBrightness()
        {
            for (int i = _configuration.MinBrightness; i <= _configuration.MaxBrightness; i += 10)
            {
                _elgator.SetBrightness(i);
                await Task.Delay(200).ConfigureAwait(false);
            }
        }

        private static async Task TurnOff()
        {
            await _elgator.SetOff().ConfigureAwait(false);
        }

        private static async Task TurnOn()
        {
            await _elgator.SetOn().ConfigureAwait(false);
        }

        private static async Task LoopTemperature()
        {
            for (int i = _configuration.MinTemperature; i < _configuration.MaxTemperature; i += 10)
            {
                if (i == _configuration.MaxTemperature - 1)
                {
                    i = _configuration.MaxTemperature;
                }

                _elgator.SetTemperature(i);

                await Task.Delay(100).ConfigureAwait(false);
            }
        }
    }
}