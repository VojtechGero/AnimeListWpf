using AnimeListWpf.Services;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace AnimeListWpf.Views.UserControls;

/// <summary>
/// Interaction logic for MenuBar.xaml
/// </summary>
public partial class MenuBar : UserControl
{
    MainWindow _mainWindow;
    FileHandler _fileHandler;
    public MenuBar()
    {
        InitializeComponent();
        _mainWindow = (MainWindow)Application.Current.MainWindow;
        _fileHandler = FileHandler.OpenFile();
    }

    private void MenuOrderName_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.changeSortType(SortType.Aplhabetical);
    }

    private void MenuOrderActual_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.changeSortType(SortType.None);
    }

    private void MenuOrderFinished_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.changeSortType(SortType.Finished);
    }

    private void MenuOrderNew_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.changeSortType(SortType.AiredAscending);
    }

    private void MenuOrderOld_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.changeSortType(SortType.AiredDescending);
    }

    private void MenuOrderScore_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.changeSortType(SortType.Score);
    }

    private string? ChooseFolder()
    {
        var openFolder = new OpenFolderDialog();
        if (openFolder.ShowDialog() == true)
        {
            return openFolder.FolderName;
        }
        return null;
    }
    private string? ChooseFile(string fileType)
    {
        var openFile = new OpenFileDialog();
        openFile.InitialDirectory = SpecialDirectories.Desktop;
        openFile.Filter = $"{fileType} files (*.{fileType})|*.{fileType}|All files (*.*)|*.*";
        openFile.CheckFileExists = true;
        bool? result = openFile.ShowDialog();
        if (result == true)
        {
            return openFile.FileName;
        }
        return null;
    }

    private void MenuExportJson_Click(object sender, RoutedEventArgs e)
    {
        var path = ChooseFolder();
        if (path is not null)
        {
            _fileHandler.ExportJson(path);
        }
    }

    private void MenuExportTxt_Click(object sender, RoutedEventArgs e)
    {
        var path = ChooseFolder();
        if (path is not null)
        {
            var names = _mainWindow.RawContent.Select(x => x.Name).ToList();
            _fileHandler.ExportTxt(path, names);
        }
    }

    private void MenuUtilRandom_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.RandomSelect();
    }

    private void MenuUtilSelectAll_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.SelectAll();
    }

    private void MenuUtilRemoveDupes_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.RemoveDuplicates();
    }

    private void MenuUtilStats_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.ShowStats();
    }

    private void MenuAddAnime_Click(object sender, RoutedEventArgs e)
    {
        AddWindow addWindow = new AddWindow(true);
        addWindow.ShowDialog();
    }

    private void MenuAddManga_Click(object sender, RoutedEventArgs e)
    {
        AddWindow addWindow = new AddWindow(false);
        addWindow.ShowDialog();
    }

    private void MenuAddFromTxt_Click(object sender, RoutedEventArgs e)
    {
        string? inputFile = ChooseFile("txt");
        if (inputFile is not null) _mainWindow.FromTxt(inputFile);
    }

    private void MenuAddFromJson_Click(object sender, RoutedEventArgs e)
    {
        string? inputFilePath = ChooseFile("json");
        if (inputFilePath is not null) _mainWindow.FromJson(inputFilePath);
    }
}
