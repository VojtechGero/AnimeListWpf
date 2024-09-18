using AnimeListWpf.Models;
using AnimeListWpf.Services;
using AnimeListWpf.Views.UserControls;
using ContentList.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AnimeListWpf.Views;

/// <summary>
/// Interaction logic for AddWindow.xaml
/// </summary>
public partial class AddWindow : Window
{
    bool Parsing, IdError, IsAnime;
    AddContent[] addContents;
    List<AContent> contentList = new();
    MalContext _malCtx;
    MainWindow _mainWindow;

    DispatcherTimer _timer;
    public AddWindow(bool IsAnime)
    {
        InitializeComponent();
        this.IsAnime = IsAnime;
        _mainWindow = (MainWindow)Application.Current.MainWindow;
        _malCtx = new MalContext();
        addContents = [
            AddContent0,
            AddContent1,
            AddContent2,
            AddContent3,
            AddContent4
        ];
        IdError = Parsing = false;
        updateForm();
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(1000);
        _timer.IsEnabled = true;
        _timer.Tick += parseInput;

    }
    private async void parseInput(object? state, EventArgs e)
    {
        TimerStop();
        Parsing = true;
        IdError = false;
        if (string.IsNullOrWhiteSpace(SearchBars.IdField.Text) && !string.IsNullOrWhiteSpace(SearchBars.SearchBox.Text))
        {
            await parseSearch();
        }
        else if (!string.IsNullOrWhiteSpace(SearchBars.IdField.Text))
        {
            await parseId();
        }
        else contentList.Clear();
        updateForm();
        Parsing = false;
        updateForm();

    }
    private async Task parseSearch()
    {
        contentList.Clear();
        var content = new List<AContent>();
        if (IsAnime)
        {
            content = await _malCtx.searchAnime(SearchBars.SearchBox.Text);
        }
        else
        {
            content = await _malCtx.searchManga(SearchBars.SearchBox.Text);
        }
        if (content is null)
        {
            MessageBox.Show("Server or internet error");
            this.Close();
        }
        else
        {
            contentList.AddRange(content);
            contentList = StringOps.sortSearch(contentList, SearchBars.SearchBox.Text);
        }
    }
    private async Task parseId()
    {
        if (long.TryParse(SearchBars.IdField.Text, out long id))
        {
            AContent contentFromId;
            if (IsAnime)
            {
                contentFromId = await _malCtx.GetAnimeId(id);
            }
            else
            {
                contentFromId = await _malCtx.GetMangaId(id);
            }
            if (contentFromId != null)
            {
                contentList.Clear();
                contentList.Add(contentFromId);
                IdError = false;
            }
            else
            {
                IdError = true;
            }
        }
        else IdError = true;

    }
    private void reDrawButtons()
    {
        foreach (var item in addContents)
        {
            item.Visibility = Visibility.Hidden;
        }
        for (int i = 0; i < contentList.Count; i++)
        {
            addContents[i].Visibility = Visibility.Visible;
            addContents[i].Name.Text = contentList[i].Name;
        }

    }
    private void TimerStartOrRestart()
    {
        _timer.Stop();
        _timer.Start();
    }
    private void TimerStop()
    {
        _timer.Stop();
    }

    private void updateForm()
    {
        errorCheck();
        reDrawButtons();
    }
    private void errorCheck()
    {
        if (IdError)
        {
            contentList.Clear();
            SearchBars.ErrorLabel.Text = "Invalid Id";
            SearchBars.ErrorLabel.Visibility = Visibility.Visible;
        }
        else SearchBars.ErrorLabel.Visibility = Visibility.Hidden;

    }
    public void ButtonPressed(Button sender)
    {
        int index = addContents.Select(x => x.AddButton).ToList().IndexOf(sender);
        AContent toAdd = contentList[index];
        _mainWindow.AddContent(toAdd);
        this.Close();
    }

    public void IdFieldUpdate()
    {
        if (!Parsing)
        {
            TimerStartOrRestart();
        }
    }
    public void SearchBoxUpdate()
    {
        if (!Parsing)
        {
            TimerStartOrRestart();
        }
    }
}
