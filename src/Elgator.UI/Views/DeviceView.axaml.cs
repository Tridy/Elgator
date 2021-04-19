using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Elgator.UI.Views
{
    public class DeviceView : UserControl
    {
        public DeviceView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            AddWindowMouseDragHandling();
        }

        private void AddWindowMouseDragHandling()
        {
            this.FindControl<Border>("mainBorder").PointerPressed += (i, e) =>
            {
                Connector.HostWindow?.PlatformImpl?.BeginMoveDrag(e);
            };
        }
    }
}