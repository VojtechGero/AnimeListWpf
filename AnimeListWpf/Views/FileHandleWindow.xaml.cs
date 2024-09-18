using AnimeListWpf.Models;
using AnimeListWpf.Services;
using ContentList.Services;
using System.Windows;

namespace AnimeListWpf.Views;

/// <summary>
/// Interaction logic for FileHandleWindow.xaml
/// </summary>
public partial class FileHandleWindow : Window
{
    List<string> names;
    MainWindow _mainWindow;
    bool stopParsing;
    public FileHandleWindow(string path)
    {
        InitializeComponent();
        _mainWindow = (MainWindow)Application.Current.MainWindow;
        ParsedFileName.Text = "Parsing: " + StringOps.getFileName(path);
        names = FileHandler.GetLines(path);
        stopParsing = false;
        setupProgressBar();
    }

    void setupProgressBar()
    {
        progressBar.Value = 0;
        progressBar.Maximum = names.Count * 10;
    }

    private async Task FinishProgressBar()
    {
        while (progressBar.Value < progressBar.Maximum)
        {
            {
                progressBar.Value += 1;
                await Task.Delay(50);
            }
        }
    }
    private async void stepProgressBar()
    {
        for (int i = 0; i < 10; i++)
        {
            progressBar.Value += 1;
            await Task.Delay(50);
        }
    }
    AContent bestCandidate(List<AContent> content, string query)
    {
        if (!content.Any()) return null;
        content = StringOps.sortSearch(content, query);
        return content.First();
    }

    async Task parseLine(string name, MalContext mal)
    {
        List<AContent> content = new List<AContent>();
        string query = name.Trim();
        ContentNameLabel.Text = "Processing: " + query;
        content.Clear();
        content.AddRange(await mal.searchAnime(query));
        content.AddRange(await mal.searchManga(query));
        AContent candidate = bestCandidate(content, query);
        if (candidate != null)
        {
            _mainWindow.AddContent(candidate);
        }
        stepProgressBar();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        MalContext mal = new MalContext();
        foreach (string name in names)
        {
            if (stopParsing) break;
            await parseLine(name, mal);
        }
        await FinishProgressBar();
        Close();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        stopParsing = true;
    }
}
