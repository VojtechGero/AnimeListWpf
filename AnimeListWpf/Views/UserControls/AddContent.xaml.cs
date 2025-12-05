using AnimeListWpf.Services;
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
        ImageService _imageService;

        public AddContent()
        {
            InitializeComponent();
            _addWindow = Application.Current.Windows.OfType<AddWindow>().SingleOrDefault();
        }
        public void SetImageService(ImageService imageService)
        {
            _imageService = imageService;
        }
        public async Task LoadImage(string imageUrl)
        {
            // Important for UI threading
            var bitmap = await _imageService.DownloadImageToBitmap(imageUrl);
            ContentImage.Source = bitmap;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _addWindow.ButtonPressed((Button)sender);
        }
    }
}
