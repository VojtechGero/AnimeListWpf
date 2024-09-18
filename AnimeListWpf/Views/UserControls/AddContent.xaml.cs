using System.Windows;
using System.Windows.Controls;

namespace AnimeListWpf.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddContent.xaml
    /// </summary>
    public partial class AddContent : UserControl
    {
        AddWindow _addWindow;
        public AddContent()
        {
            InitializeComponent();
            _addWindow = Application.Current.Windows.OfType<AddWindow>().SingleOrDefault();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _addWindow.ButtonPressed((Button)sender);
        }
    }
}
