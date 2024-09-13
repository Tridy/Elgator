using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Elgator.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            this.FindControl<Button>("CloseButton")!.Click += delegate
            {
                Close();
            };
        }
    }
}