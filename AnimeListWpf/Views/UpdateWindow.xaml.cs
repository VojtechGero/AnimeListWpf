using AnimeListWpf.Models;
using ContentList.Services;
using System.Windows;

namespace AnimeListWpf.Views;

/// <summary>
/// Interaction logic for UpdateWindow.xaml
/// </summary>
public partial class UpdateWindow : Window
{
    public List<AContent> content;
    bool stopParsing;
    int progress;
    public UpdateWindow(List<AContent> content)
    {
        InitializeComponent();
        progress = 0;
        stopParsing = false;
        this.content = content;
        setupProgressBar();
    }
    void setupProgressBar()
    {
        progressBar.Value = 0;
        progressBar.Maximum = content.Count * 10;
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

    private async Task<AContent> update(AContent oldContent, MalContext malContext)
    {
        ContentNameLabel.Text = $"Updating {progress}/{content.Count} {oldContent.Name}";
        AContent newContent;
        if (oldContent.IsAnime)
        {
            newContent = await malContext.GetAnimeId(oldContent.Id);
        }
        else
        {
            newContent = await malContext.GetMangaId(oldContent.Id);
        }
        if (newContent.OtherName == oldContent.Name)
        {
            (newContent.Name, newContent.OtherName) = (newContent.OtherName, newContent.Name);
        }
        newContent.InProgress = oldContent.InProgress;
        stepProgressBar();
        return newContent;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        stopParsing = true;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {

        MalContext mal = new MalContext();
        ContentNameLabel.Visibility = Visibility.Visible;
        for (int i = 0; i < content.Count; i++)
        {
            if (stopParsing) break;
            progress++;
            content[i] = await update(content[i], mal);
        }
        await FinishProgressBar();
        this.Close();
    }
}
