using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Elgator.UI.ViewModels
{
    public class DeviceViewModel : ViewModelBase, IDisposable
    {
        private string _name = "N/A";
        private int _brightness = 50;
        private int _temperature = 200;
        private bool _isOn = false;

        private Elgator _elgator;

        public string Name => _name;

        public ReactiveCommand<bool, Unit> OnCommand { get; }

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
            get
            {
                return _brightness;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _brightness, value);
            }
        }

        public int Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _temperature, value);
            }
        }

        public Configuration Configuration { get; set; }

        internal static DeviceViewModel FromConfiguration(Configuration configuration)
        {
            return new DeviceViewModel(configuration);
        }

        public DeviceViewModel(Configuration configuration)
        {
            Configuration = configuration;

            _elgator = Elgator.FromConfiguration(configuration);

            var info = _elgator.GetAccessoryInfo().ConfigureAwait(false).GetAwaiter().GetResult();
            _name = info.Name;

            var startingState = _elgator.GetState().ConfigureAwait(false).GetAwaiter().GetResult();

            _isOn = startingState.Lights[0].IsOn == 1;
            _brightness = startingState.Lights[0].Brightness;
            _temperature = startingState.Lights[0].Temperature;

            OnCommand = ReactiveCommand.CreateFromTask<bool, Unit>(OnToggleCommand);

            SubscribeToChanges();

            _elgator.Start();
        }

        private void SubscribeToChanges()
        {
            this.WhenPropertyChanged(x => x.Temperature, notifyOnInitialValue: false)
                .Subscribe(changed =>
                {
                    _elgator.SetTemperature(changed.Value);
                });

            this.WhenPropertyChanged(x => x.Brightness, notifyOnInitialValue: false)
                .Subscribe(changed =>
                {
                    _elgator.SetBrightness(changed.Value);
                });
        }

        private async Task<Unit> OnToggleCommand(bool isOn)
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

        public void Dispose()
        {
            _elgator?.Dispose();
        }
    }
}