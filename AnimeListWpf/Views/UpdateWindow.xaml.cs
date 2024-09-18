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
    public UpdateWindow(List<AContent> content)
    {
        InitializeComponent();
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

    private async Task<AContent> update(AContent toUpdate, MalContext malContext)
    {
        ContentNameLabel.Text = $"Updating {toUpdate.Name}";
        AContent newContent;
        if (toUpdate.IsAnime)
        {
            newContent = await malContext.GetAnimeId(toUpdate.Id);
        }
        else
        {
            newContent = await malContext.GetMangaId(toUpdate.Id);
        }
        if (newContent.OtherName == toUpdate.Name)
        {
            (newContent.Name, newContent.OtherName) = (newContent.OtherName, newContent.Name);
        }
        newContent.InProgress = toUpdate.InProgress;
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
            content[i] = await update(content[i], mal);
        }
        await FinishProgressBar();
        this.Close();
    }
}
