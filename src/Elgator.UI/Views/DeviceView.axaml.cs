using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Elgator.UI.Views
{
    public partial class DeviceView : UserControl
    {
        private readonly Border _borderControl;
        private Window? _mainWindow;
        private bool _mouseDownForWindowMoving = false;
        private PointerPoint _originalPoint;
        
        public DeviceView()
        {
            InitializeComponent();
            _borderControl = this.FindControl<Border>("mainBorder")!;
            AddWindowMouseDragHandling();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _mainWindow = this.FindControl<Window>("mainWindow");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AddWindowMouseDragHandling()
        {
            _borderControl.PointerMoved += Border_OnPointerMoved;
            _borderControl.PointerPressed += Border_OnPointerPressed;
            _borderControl.PointerReleased += Border_OnPointerReleased;
        }
        
        private void Border_OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (!_mouseDownForWindowMoving) return;

            PointerPoint currentPoint = e.GetCurrentPoint(this);
            _mainWindow!.Position = new PixelPoint(_mainWindow.Position.X + (int)(currentPoint.Position.X - _originalPoint.Position.X),
                _mainWindow.Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
        }

        private void Border_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            Visual? window = this.GetVisualAncestors().FirstOrDefault(i => i is Window);

            if (window is not null)
            {
                _mainWindow = (Window)window;
            }
            
            if (_mainWindow!.WindowState == WindowState.Maximized || _mainWindow.WindowState == WindowState.FullScreen) return;

            _mouseDownForWindowMoving = true;
            _originalPoint = e.GetCurrentPoint(this);
        }

        private void Border_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            _mouseDownForWindowMoving = false;
            _mainWindow = null;
        }
    }
}