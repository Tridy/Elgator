using Microsoft.Extensions.Configuration;
using Refit;
using System.Threading.Tasks;

namespace Elgator
{
    public class Elgator
    {
        private IElgato _elgato;

        private Configuration _configuration;

        private Elgator(IConfiguration configuration)
        {
            InitializeConfiguration(configuration);

            var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer());
            _elgato = RestService.For<IElgato>(_configuration.Url, refitSettings);
        }

        private void InitializeConfiguration(IConfiguration configuration)
        {
            _configuration = new Configuration
            {
                Url = configuration["Url"],
                MinBrightness = int.Parse(configuration["MinBrightness"]),
                MaxBrightness = int.Parse(configuration["MaxBrightness"]),
                MinTemperature = int.Parse(configuration["MinTemperature"]),
                MaxTemperature= int.Parse(configuration["MaxTemperature"]),
            };
        }

        public static Elgator FromConfiguration(IConfiguration configuration)
        {
            
            return new Elgator(configuration);
        }

        public async Task<AccessoryInfo> GetAccessoryInfo()
        {
            var accessoryInfo = await _elgato.GetAccessoryInfo().ConfigureAwait(false);
            return accessoryInfo;
        }

        public async Task SetBrightness(int i)
        {
            await SetBrightnessValue(i).ConfigureAwait(false);
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

        private async Task<StateChangeResult> SetBrightnessValue(int brightness)
        {
            if(brightness < _configuration.MinBrightness)
            {
                brightness = _configuration.MinBrightness;
            }

            if(brightness > _configuration.MaxBrightness)
            {
                brightness = _configuration.MaxBrightness;
            }

            var state = new StateInfo<Brightness>
            {
                NumberOfLights = 1,
                Changes = new[] { new Brightness { Value = brightness } }
            };

            var jsonState = System.Text.Json.JsonSerializer.Serialize(state);

            StateChangeResult result = await _elgato.SetState(jsonState).ConfigureAwait(false);

            return result;
        }

        public async Task<StateChangeResult> SetTemperature(int temperature)
        {
            if(temperature < _configuration.MinTemperature)
            {
                temperature = _configuration.MinTemperature;
            }

            if(temperature > _configuration.MaxTemperature)
            {
                temperature = _configuration.MaxTemperature;
            }

            var state = new StateInfo<Temperature>
            {
                NumberOfLights = 1,
                Changes = new[] { new Temperature { Value = temperature } }
            };

            var jsonState = System.Text.Json.JsonSerializer.Serialize(state);

            StateChangeResult result = await _elgato.SetState(jsonState).ConfigureAwait(false);

            return result;
        }
    }
}
