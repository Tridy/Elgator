using Refit;
using System;
using System.Threading.Tasks;

namespace Elgator
{
    public class Elgator : IDisposable
    {
        private IElgato _elgato;

        private Configuration _configuration;

        private System.Timers.Timer _timer;

        private int _currentBrightness;
        private int _currentTemperature;

        private int _newBrightness;
        private int _newTemperature;

        private Elgator(Configuration configuration)
        {
            _configuration = configuration;
            var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer());
            _elgato = RestService.For<IElgato>(_configuration.Url, refitSettings);
        }

        public void Start(StateChangeResult startingState)
        {
            _currentBrightness = startingState.Lights[0].Brightness;
            _newBrightness = _currentBrightness;
            _currentTemperature = startingState.Lights[0].Temperature;
            _newTemperature = _currentTemperature;
            StartTimer();
        }

        private void StartTimer()
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = _configuration.UpdateInMilliseconds;
            _timer.Elapsed += OnTimerTick;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_newBrightness != _currentBrightness)
            {
                _currentBrightness = _newBrightness;
                SetBrightnessValue(_newBrightness).GetAwaiter().GetResult();
            }

            if (_newTemperature != _currentTemperature)
            {
                _currentTemperature = _newTemperature;
                SetTemperatureValue(_newTemperature).GetAwaiter().GetResult();
            }
        }

        public static Elgator FromConfiguration(Configuration configuration)
        {
            return new Elgator(configuration?? throw new ArgumentNullException(nameof(configuration)));
        }

        public async Task<AccessoryInfo> GetAccessoryInfo()
        {
            var accessoryInfo = await _elgato.GetAccessoryInfo().ConfigureAwait(false);
            return accessoryInfo;
        }

        public void SetBrightness(int brightness)
        {
            if (brightness < _configuration.MinBrightness)
            {
                brightness = _configuration.MinBrightness;
            }

            if (brightness > _configuration.MaxBrightness)
            {
                brightness = _configuration.MaxBrightness;
            }

            if (brightness != _currentBrightness)
            {
                _newBrightness = brightness;
            }
        }

        public void SetTemperature(int temperature)
        {
            if (temperature < _configuration.MinTemperature)
            {
                temperature = _configuration.MinTemperature;
            }

            if (temperature > _configuration.MaxTemperature)
            {
                temperature = _configuration.MaxTemperature;
            }

            if (temperature != _currentTemperature)
            {
                _newTemperature = temperature;
            }
        }

        public async Task SetOn()
        {
            var state = new StateInfo<LightOn>
            {
                NumberOfLights = 1,
                Changes = new[] { new LightOn() }
            };

            await SetState(state).ConfigureAwait(false);
        }

        public async Task SetOff()
        {
            var state = new StateInfo<LightOff>
            {
                NumberOfLights = 1,
                Changes = new[] { new LightOff() }
            };

            await SetState(state).ConfigureAwait(false);
        }

        private async Task<StateChangeResult> SetState<T>(StateInfo<T> stateToSet)
        {
            var jsonStateMS = System.Text.Json.JsonSerializer.Serialize(stateToSet);
            StateChangeResult result = await _elgato.SetState(jsonStateMS).ConfigureAwait(false);
            return result;
        }

        private async Task<StateChangeResult> SetTemperatureValue(int temperature)
        {
            var state = new StateInfo<Temperature>
            {
                NumberOfLights = 1,
                Changes = new[] { new Temperature { Value = temperature } }
            };

            var jsonState = System.Text.Json.JsonSerializer.Serialize(state);

            StateChangeResult result = await _elgato.SetState(jsonState).ConfigureAwait(false);

            return result;
        }

        private async Task<StateChangeResult> SetBrightnessValue(int brightness)
        {
            var state = new StateInfo<Brightness>
            {
                NumberOfLights = 1,
                Changes = new[] { new Brightness { Value = brightness } }
            };

            var jsonState = System.Text.Json.JsonSerializer.Serialize(state);

            StateChangeResult result = await _elgato.SetState(jsonState).ConfigureAwait(false);

            return result;
        }

        public async Task<StateChangeResult> GetState()
        {
            var state = new StateInfo<Brightness>
            {
                NumberOfLights = 1,
                //Changes = new[] { new Brightness { Value = brightness } }
            };

            var jsonState = System.Text.Json.JsonSerializer.Serialize(state);

            StateChangeResult result = await _elgato.GetState(jsonState).ConfigureAwait(false);

            return result;
        }

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _elgato = null;
        }
    }
}