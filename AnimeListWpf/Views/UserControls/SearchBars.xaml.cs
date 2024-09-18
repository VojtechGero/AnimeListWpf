using System.Windows;
using System.Windows.Controls;

namespace AnimeListWpf.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SearchBars.xaml
    /// </summary>
    public partial class SearchBars : UserControl
    {
        AddWindow _addWindow;
        public SearchBars()
        {
            InitializeComponent();
            _addWindow = Application.Current.Windows.OfType<AddWindow>().SingleOrDefault();
        }

        private void IdField_TextChanged(object sender, TextChangedEventArgs e)
        {
            _addWindow.IdFieldUpdate();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _addWindow.SearchBoxUpdate();
        }

    }
}
