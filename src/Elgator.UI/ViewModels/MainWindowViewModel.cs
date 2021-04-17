using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Elgator.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<DeviceViewModel> Devices { get; private set; }

        public MainWindowViewModel()
        {
            var configurations = GetConfigurations();

            var devices = configurations.Select(c => DeviceViewModel.FromConfiguration(c));
            Devices = new ObservableCollection<DeviceViewModel>(devices);
        }

        private static IEnumerable<Configuration> GetConfigurations()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("app.config.json")
                .AddJsonFile("app.config.local.json", optional: true)
                .Build();

            var configurations = Configuration.FromConfiguration(config);

            return configurations;
        }
    }
}