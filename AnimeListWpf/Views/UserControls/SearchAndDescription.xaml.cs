using AnimeListWpf.Models;
using System.Windows;
using System.Windows.Controls;

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
    public void SwapNames()
    {
        Description.SwapNames();
    }
    private void ShowDescription()
    {
        if (content is null) return;
        Description.UpdateDescription(content);
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
        Description.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
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
