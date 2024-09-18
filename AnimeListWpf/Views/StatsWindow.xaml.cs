using AnimeListWpf.Models;
using System.Windows;

namespace AnimeListWpf.Views;

/// <summary>
/// Interaction logic for StatsWindow.xaml
/// </summary>
public partial class StatsWindow : Window
{
    public StatsWindow(List<AContent> list)
    {
        InitializeComponent();
        StatsText.Text = Stats(list);
    }

    private static string Stats(List<AContent> list)
    {
        string output = "";
        output += $"Total count: {list.Count}\n";
        int animes = list.Where(x => x.IsAnime).Count();
        int mangas = list.Where(x => !x.IsAnime).Count();
        output += $"Animes: {animes}\nMangas: {mangas}";
        return output;
    }
}
