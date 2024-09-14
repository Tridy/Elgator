using DynamicData.Binding;

using System;

using CommunityToolkit.Mvvm.ComponentModel;

using ReactiveUI.Fody.Helpers;

namespace Elgator.UI.ViewModels
{
    public class DeviceViewModel : ViewModelBase, IDisposable
    {
        private Elgator _elgator;
        
        [Reactive]
        private string Name { get; set; }

        [Reactive]
        private bool IsOn { get; set; }

        [Reactive] 
        private int Brightness { get; set; }
        
        [Reactive] 
        private int Temperature { get; set; }

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
            Name = info.Name;

            var startingState = _elgator.GetState().ConfigureAwait(false).GetAwaiter().GetResult();

            IsOn = startingState.Lights[0].IsOn == 1;
            Brightness = startingState.Lights[0].Brightness;
            Temperature = startingState.Lights[0].Temperature;

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

            this.WhenPropertyChanged(x => x.IsOn, notifyOnInitialValue: false)
                .Subscribe(isOn =>
                {
                    if (isOn.Value)
                    {
                        _elgator.SetOn().ConfigureAwait(false);
                    }
                    else
                    {
                        _elgator.SetOff().ConfigureAwait(false);
                    }
                });
        }

        public void Dispose()
        {
            _elgator?.Dispose();
        }
    }
}