using Elgator;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ElgatoCommands
{
    internal class Program
    {
        private static Elgator.Elgator? _elgator1;

        private static Configuration? _configuration1;

        private static Elgator.Elgator Elgator
        {
            get => _elgator1 ?? throw new NullReferenceException();
        }
        
        private static Configuration Configuration
        {
            get => _configuration1 ?? throw new NullReferenceException();
        }

        private static async Task Main(string[] args)
        {
            _configuration1 = GetConfiguration();
            
            using (_elgator1 = global::Elgator.Elgator.FromConfiguration(Configuration))
            {
                if (await ProcessParametersAsync(args))
                {
                    return;
                }
                
                await Demo().ConfigureAwait(false);
            }

            Console.WriteLine("Any key to exit.");

            Console.ReadKey();
        }

        private static async Task<bool> ProcessParametersAsync(string[] args)
        {
            if (args.Length == 0)
            {
                return false;
            }

            if (args[0] == "--set-off")
            {
                await TurnOff().ConfigureAwait(false);
                return true;
            }
            
            if (args[0] == "--set-on")
            {
                await TurnOn().ConfigureAwait(false);
                return true;
            }

            return false;

        }

        private static async Task Demo()
        {
            Elgator.AccessoryInfo info = await Elgator.GetAccessoryInfo().ConfigureAwait(false);

            Console.WriteLine("==========================================");
            Console.WriteLine($"{nameof(info.Product)}: {info.Product}");
            Console.WriteLine($"{nameof(info.Name)}: {info.Name}");
            Console.WriteLine($"{nameof(info.SerialNumber)}: {info.SerialNumber}");
            Console.WriteLine($"{nameof(info.FirmwareBuild)}: {info.FirmwareBuild}");
            Console.WriteLine($"{nameof(info.FirmwareVersion)}: {info.FirmwareVersion}");
            Console.WriteLine($"{nameof(info.BoardType)}: {info.BoardType}");
            Console.WriteLine($"{nameof(info.Features)}: { string.Join(',', info.Features)}");
            Console.WriteLine("==========================================");

            Elgator.Start();

            await TurnOn().ConfigureAwait(false);

            await Task.Delay(1000).ConfigureAwait(false);

            await LoopBrightness().ConfigureAwait(false);

            await Task.Delay(1000).ConfigureAwait(false);

            await LoopTemperature().ConfigureAwait(false);

            await TurnOff().ConfigureAwait(false);
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
            for (int i = Configuration.MinBrightness; i <= Configuration.MaxBrightness; i += 10)
            {
                Elgator.SetBrightness(i);
                await Task.Delay(200).ConfigureAwait(false);
            }
        }

        private static async Task TurnOff()
        {
            await Elgator.SetOff().ConfigureAwait(false);
        }

        private static async Task TurnOn()
        {
            await Elgator.SetOn().ConfigureAwait(false);
        }

        private static async Task LoopTemperature()
        {
            for (int i = Configuration.MinTemperature; i < Configuration.MaxTemperature; i += 10)
            {
                if (i == Configuration.MaxTemperature - 1)
                {
                    i = Configuration.MaxTemperature;
                }

                Elgator.SetTemperature(i);

                await Task.Delay(100).ConfigureAwait(false);
            }
        }
    }
}