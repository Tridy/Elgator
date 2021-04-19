using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Elgator.UI.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            Connector.HostWindow = this;

            this.FindControl<Button>("CloseButton").Click += delegate
            {
                Close();
            };
        }
    }
}