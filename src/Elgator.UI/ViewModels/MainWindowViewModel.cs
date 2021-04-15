using Microsoft.Extensions.Configuration;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Elgator.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Configuration _configuration;

        private int _brightness = 50;
        private int _temperature = 200;

        private Elgator _elgator;

        private string _greeting;

        private bool _isOn;

        public ReactiveCommand<bool, Unit> OnCommand { get; }

        public MainWindowViewModel()
        {
            _configuration = GetConfiguration();

            _elgator = Elgator.FromConfiguration(_configuration);
            var info = _elgator.GetAccessoryInfo().ConfigureAwait(false).GetAwaiter().GetResult();
            _greeting = info.Name;
            var state = _elgator.GetState().ConfigureAwait(false).GetAwaiter().GetResult();
            _isOn = state.Lights[0].IsOn == 1;

            OnCommand = ReactiveCommand.CreateFromTask<bool, Unit>(OnOnCommand);
        }

        private async Task<Unit> OnOnCommand(bool isOn)
        {
            if (isOn)
            {
                await _elgator.SetOn().ConfigureAwait(false);
            }
            else
            {
                await _elgator.SetOff().ConfigureAwait(false);
            }

            return Unit.Default;
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

        public bool IsOn
        {
            get => _isOn;
            set
            {
                this.RaiseAndSetIfChanged(ref _isOn, value);

                
            }
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