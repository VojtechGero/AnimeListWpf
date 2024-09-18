using AnimeListWpf.Models;
using AnimeListWpf.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace AnimeListWpf.Views.UserControls;

/// <summary>
/// Interaction logic for SearchAndDescription.xaml
/// </summary>
public partial class SearchAndDescription : UserControl
{
    MainWindow _mainWindow;
    AContent? content;
    public SearchAndDescription()
    {
        InitializeComponent();
        content = null;
        _mainWindow = (MainWindow)Application.Current.MainWindow;
        RemoveButton.Visibility = Visibility.Hidden;
        RefreshButton.Visibility = Visibility.Hidden;
        WatchButton.Visibility = Visibility.Hidden;
        SwapButton.Visibility = Visibility.Hidden;
        showDescription(false);
    }

    private void SwapButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _mainWindow.SwapNames();
    }

    private void WatchButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _mainWindow.WatchItem();
    }

    private void RefreshButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _mainWindow.RefreshContent();
    }

    private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        HideDescription();
        _mainWindow.RemoveItems();
    }

    private void MalLogo_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (content is null) return;
        Process.Start(new ProcessStartInfo
        {
            FileName = StringOps.GetLink(content),
            UseShellExecute = true
        });
    }
    public void HideDescription()
    {
        WatchButton.Visibility = Visibility.Hidden;
        SwapButton.Visibility = Visibility.Hidden;
        RemoveButton.Visibility = Visibility.Visible;
        RefreshButton.Visibility = Visibility.Visible;
        showDescription(false);
    }
    public void DisplayContent(AContent content)
    {
        this.content = content;
        ShowDescription();
    }

    private void ShowDescription()
    {
        if (content is null) return;
        NameLabel.Text = content.Name;
        Description.Text = content.Description();
        if (content.OtherName is not null)
        {
            SwapButton.Visibility = Visibility.Visible;
        }
        else SwapButton.Visibility = Visibility.Hidden;
        WatchButton.Visibility = Visibility.Visible;
        RemoveButton.Visibility = Visibility.Visible;
        RefreshButton.Visibility = Visibility.Visible;
        showDescription(true);
        ProgressButton();
    }

    private void showDescription(bool show)
    {
        Visibility visibility;
        if (show) visibility = Visibility.Visible;
        else visibility = Visibility.Hidden;
        Description.Visibility = visibility;
        NameLabel.Visibility = visibility;
        MalLogo.Visibility = visibility;
    }
    private void ProgressButton()
    {

        if (content is null) return;
        string action = content.IsAnime ? "Watch" : "Read";
        if (content.InProgress)
        {
            action = "Un" + action;
        }
        WatchButton.Content = action;
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
            popup.IsOpen = false; // Close the popup
            timer.Stop();         // Stop the timer
        };
        timer.Start();
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(SearchBox.Text))
        {
            SearchBoxTemp.Text = "Name search...";
        }
        else
        {
            SearchBoxTemp.Text = "";
        }
        _mainWindow.SearchBoxChanged(SearchBox.Text);
    }
}
