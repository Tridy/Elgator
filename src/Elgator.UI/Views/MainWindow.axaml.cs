using Avalonia.Controls;

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