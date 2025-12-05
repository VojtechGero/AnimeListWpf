using AnimeListWpf.Models;
using AnimeListWpf.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace AnimeListWpf.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ContentDescription.xaml
    /// </summary>
    public partial class ContentDescription : UserControl
    {
        AContent? content;
        public ContentDescription()
        {
            InitializeComponent();
        }
        private void NameLabel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (content is null) return;
            Clipboard.SetText(content.Name);

            var mousePosition = Mouse.GetPosition(this);
            var screenPosition = PointToScreen(mousePosition);

            // Convert screen position to logical pixels
            var dpi = VisualTreeHelper.GetDpi(this);
            var logicalScreenPosition = new Point(
                screenPosition.X / dpi.DpiScaleX,
                screenPosition.Y / dpi.DpiScaleY
            );

            // Create a Popup
            Popup popup = new Popup
            {
                Placement = PlacementMode.AbsolutePoint,
                PlacementTarget = this,
                HorizontalOffset = logicalScreenPosition.X,
                VerticalOffset = logicalScreenPosition.Y + SystemParameters.CursorHeight / 2,
                Child = new TextBlock
                {
                    Text = "Copied to Clipboard!",
                    Background = Brushes.LightYellow,
                    Padding = new Thickness(5)
                }
            };

            // Open the popup
            popup.IsOpen = true;

            // Set a timer to close the popup after 1 second
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, args) =>
            {
                popup.IsOpen = false;
                timer.Stop();
            };
            timer.Start();
        }
        public void UpdateDescription(AContent content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            this.content = content;
            this.DataContext = content;
            Description.Text = content.Description();
        }

        private void MalLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (content is null) return;
            Process.Start(new ProcessStartInfo
            {
                FileName = StringOps.GetLink(content),
                UseShellExecute = true
            });
        }

        public void SwapNames()
        {
            NameLabel.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            OtherNameLabel.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
    }
}
