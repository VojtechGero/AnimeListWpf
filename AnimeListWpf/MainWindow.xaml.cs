using AnimeListWpf.Models;
using AnimeListWpf.Services;
using AnimeListWpf.Views;
using System.Windows;
namespace AnimeListWpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>


public partial class MainWindow : Window
{
    FileHandler _fileHandler;
    string searchQuery;
    public List<AContent> RawContent = new List<AContent>();
    public List<AContent> Sorted = new List<AContent>();
    public SortType sortOrder = SortType.None;
    public MainWindow()
    {
        InitializeComponent();
        _fileHandler = FileHandler.OpenFile();
        RawContent = _fileHandler.GetContent();
        searchQuery = "";
        sortWrite();
    }
    private void sort()
    {
        if (string.IsNullOrEmpty(searchQuery))
        {
            Sorted = SortBy(RawContent, sortOrder);
        }
        else
        {
            Sorted = StringOps.sortSearch(RawContent, searchQuery);
        }
    }
    private void writeList()
    {
        ContentList.ItemsSource = Sorted;
    }
    private void sortWrite()
    {
        sort();
        writeList();
    }
    public void changeSortType(SortType sortType)
    {
        sortOrder = sortType;
        sortWrite();
    }

    public void SearchBoxChanged(string text)
    {
        searchQuery = text;
        sortWrite();
    }

    private IEnumerable<AContent> SortContent(IEnumerable<AContent> list, SortType s) =>
        s switch
        {
            SortType.Aplhabetical => list.OrderBy(content => content.Name),
            SortType.Score => list.OrderByDescending(content => content.Score),
            SortType.Finished => list.OrderBy(content => content.NotOut),
            SortType.AiredDescending => list.OrderBy(content => content.Started),
            SortType.AiredAscending => list.OrderByDescending(content => content.Started),
            _ => list
        };

    private List<AContent> SortBy(List<AContent> Content, SortType s)
    {
        IEnumerable<AContent> Sorted = SortContent(Content, s);
        return Sorted.OrderByDescending(content => content.InProgress).ToList();
    }

    public void RandomSelect()
    {
        if (RawContent.Any())
        {
            ContentList.UnselectAll();
            var rand = new Random();
            ContentList.SelectedIndex = rand.Next(RawContent.Count);
            ContentList.ScrollIntoView(ContentList.SelectedItem);

        }
    }

    public void SelectAll()
    {
        ContentList.SelectAll();
    }

    public void RemoveDuplicates()
    {
        List<long> map = new List<long>();
        List<int> toRemove = new List<int>();
        for (int i = 0; i < RawContent.Count; i++)
        {
            if (map.Contains(RawContent[i].Id))
            {
                toRemove.Add(i);
            }
            else map.Add(RawContent[i].Id);
        }
        if (toRemove.Any())
        {
            if (ContentList.SelectedItems.Count == 1 && toRemove.Contains(ContentList.SelectedIndex))
            {
                ContentList.SelectedIndex = getDuplicate(RawContent[ContentList.SelectedIndex].Id);
            }
            toRemove.Reverse();
            foreach (int i in toRemove)
            {
                RawContent.RemoveAt(i);
            }
            _fileHandler.RemoveContents(toRemove);
            sortWrite();
        }
    }

    public void SwapNames()
    {
        int index;
        int current = ContentList.SelectedIndex;
        if (Sorted.Any())
        {
            index = getIndex(Sorted[current].Id, RawContent);
        }
        else index = current;
        (RawContent[index].Name, RawContent[index].OtherName) = (RawContent[index].OtherName, RawContent[index].Name);
        SearchAndDescription.DisplayContent(RawContent[index]);
        ContentList.Items.Refresh();
        _fileHandler.UpdateLine(index, RawContent[index]);
    }

    public void WatchItem()
    {
        int index = getIndex(Sorted[ContentList.SelectedIndex].Id, RawContent);
        RawContent[index].InProgress = !RawContent[index].InProgress;
        _fileHandler.UpdateLine(index, RawContent[index]);
        sortWrite();
        ContentList.ScrollIntoView(ContentList.SelectedItem);
        SearchAndDescription.DisplayContent(RawContent[index]);
    }

    public void RemoveItems()
    {
        List<int> selected = new List<int>();
        var selectedIds = ContentList.SelectedItems.OfType<AContent>().Select(x => x.Id);
        selected = RawContent
            .Select((item, index) => new { item, index })
            .Where(x => selectedIds.Contains(x.item.Id))
            .Select(x => x.index)
            .ToList();
        selected.Sort();
        selected.Reverse();
        foreach (int x in selected)
        {
            RawContent.RemoveAt(x);
        }
        _fileHandler.RemoveContents(selected);
        sortWrite();
    }

    public void RefreshContent()
    {
        var selectedIds = ContentList.SelectedItems.OfType<AContent>().Select(x => x.Id);
        var selected = RawContent
            .Select((item, index) => new { item, index })
            .Where(x => selectedIds.Contains(x.item.Id))
            .Select(x => x.index)
            .ToList();
        List<AContent> list = new List<AContent>();
        if (Sorted.Any())
        {
            List<int> temp = new List<int>(selected);
            selected.Clear();
            foreach (var i in temp)
            {
                selected.Add(getIndex(Sorted[i].Id, RawContent));
            }
        }
        foreach (int i in selected)
        {
            list.Add(RawContent[i]);
        }
        var window = new UpdateWindow(list);
        window.ShowDialog();
        var output = window.content;
        for (int i = 0; i < output.Count; i++)
        {
            RawContent[selected[i]] = output[i];
        }
        sortWrite();
        _fileHandler.UpdateLines(selected, RawContent);
    }

    public void AddContent(AContent content)
    {
        RawContent.Add(content);
        _fileHandler.WriteContent(content);
        sortWrite();
    }

    public void FromJson(string inputFilePath)
    {
        if (!string.IsNullOrWhiteSpace(inputFilePath))
        {
            _fileHandler.CopyJson(inputFilePath);
        }
        RawContent = _fileHandler.GetContent();
        sortWrite();
    }

    public void FromTxt(string inputFilePath)
    {
        if (!string.IsNullOrWhiteSpace(inputFilePath))
        {
            var fileHandleWindow = new FileHandleWindow(inputFilePath);
            fileHandleWindow.ShowDialog();
        }
    }

    private int getIndex(long id, List<AContent> Content)
    {
        for (int i = 0; i < Content.Count; i++)
        {
            if (Content[i].Id == id) return i;
        }
        return -1;
    }

    private int getDuplicate(long id)
    {
        for (int i = 0; i < RawContent.Count; i++)
        {
            if (RawContent[i].Id == id) return i;
        }
        return -1;
    }
    private void AnimeList_Selected(object sender, RoutedEventArgs e)
    {
        int select = ContentList.SelectedIndex;
        if (ContentList.SelectedItems.Count == 1)
        {
            if (select != -1) SearchAndDescription.DisplayContent(Sorted[select]);
        }
        else
        {
            SearchAndDescription.HideDescription();
        }
    }

    public void ShowStats()
    {
        StatsWindow stats = new StatsWindow(RawContent);
        stats.Show();
    }
}