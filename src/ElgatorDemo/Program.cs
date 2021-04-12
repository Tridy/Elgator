using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ElgatoCommands
{
    class Program
    {
        private static Elgator.Elgator _elgator;

        static async Task Main(string[] args)
        {
            IConfigurationRoot configuration = GetConfiguration();

            _elgator = Elgator.Elgator.FromConfiguration(configuration);

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

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("demo.config.json")
                .AddJsonFile("demo.config.local.json", optional: true)
                .Build();
        }

        private static async Task LoopBrightness()
        {
            for (int i = 0; i < 100; i += 10)
            {
                await _elgator.SetBrightness(i).ConfigureAwait(false);
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
            //  143(7000K) to 344(2900K)

            for (int i = 143; i < 344; i += 10)
            {
                if (i == 344 - 1)
                {
                    i = 344;
                }

                await _elgator.SetTemperature(i).ConfigureAwait(false);

                await Task.Delay(100).ConfigureAwait(false);
            }
        }
    }
}