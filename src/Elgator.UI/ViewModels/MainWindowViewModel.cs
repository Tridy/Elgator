using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;

namespace Elgator.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Configuration _configuration;

        private int _brightness = 50;
        private int _temperature = 200;

        private Elgator _elgator;

        private string _greeting;

        public ReactiveCommand<Unit, Unit> OnCommand { get; }
        public ReactiveCommand<Unit, Unit> OffCommand { get; }

        public MainWindowViewModel()
        {
            _configuration = GetConfiguration();

            OnCommand = ReactiveCommand.Create(OnOnCommand);
            OffCommand = ReactiveCommand.Create(OnOffCommand);

            _elgator = Elgator.FromConfiguration(_configuration);
            var info = _elgator.GetAccessoryInfo().ConfigureAwait(false).GetAwaiter().GetResult();
            _greeting = info.Name;
        }

        private void OnOffCommand()
        {
            _elgator.SetOff().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void OnOnCommand()
        {
            _elgator.SetOn().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static Configuration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("app.config.json")
                .AddJsonFile("app.config.local.json", optional: true)
                .Build();

            var configuration = Configuration.FromConfiguration(config);

            return configuration;
        }

        public int Brightness
        {
            get => _brightness;
            set
            {
                this.RaiseAndSetIfChanged(ref _brightness, value);
                _elgator.SetBrightness(value);
            }
        }

        public int Temperature
        {
            get => _temperature;
            set
            {
                this.RaiseAndSetIfChanged(ref _temperature, value);
                _elgator.SetTemperature(value);
            }
        }

        public int MinTemperature => _configuration.MinTemperature;
        public int MaxTemperature => _configuration.MaxTemperature;
        public int MinBrightness => _configuration.MinBrightness;
        public int MaxBrightness => _configuration.MaxBrightness;


        public string Greeting => _greeting;
    }
}