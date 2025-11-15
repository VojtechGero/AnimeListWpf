using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

        public async Task LoadImage(string imageUrl)
        {
            using var http = new HttpClient();
            var bytes = await http.GetByteArrayAsync(imageUrl);
            using var ms = new MemoryStream(bytes);
            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // Important: loads immediately
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            bitmap.Freeze(); // Important for UI threading
            ContentImage.Source = bitmap;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _addWindow.ButtonPressed((Button)sender);
        }
    }
}
