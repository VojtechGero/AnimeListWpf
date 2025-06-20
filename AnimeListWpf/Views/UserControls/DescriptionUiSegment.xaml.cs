using AnimeListWpf.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnimeListWpf.Views.UserControls
{
    /// <summary>
    /// Interaction logic for DescriptionUiSegment.xaml
    /// </summary>
    public partial class DescriptionUiSegment : UserControl
    {
        private readonly DescriptionSegment _segment;
        public DescriptionUiSegment(DescriptionSegment segment)
        {
            _segment = segment;
            InitializeComponent();
            FillDescription();
        }
        private void FillDescription()
        {
            TitleTextBox.Text = _segment.Title;
            DescriptionWrapPanel.Children.Clear();
            foreach (var item in _segment.Description)
            {
                var border = new Border();
                border.CornerRadius = new System.Windows.CornerRadius(4);
                border.BorderThickness = new System.Windows.Thickness(4);
                border.BorderBrush = Brushes.Gray;
                border.Margin = new System.Windows.Thickness(4);
                border.Background = Brushes.Gray;
                var textBlock = new TextBlock();
                textBlock.Text = item;
                textBlock.Foreground = Brushes.Black;
                textBlock.Background = Brushes.Gray;
                textBlock.Padding = new System.Windows.Thickness(3);
                textBlock.FontSize = 20;
                border.Child = textBlock;
                DescriptionWrapPanel.Children.Add(border);
            }
        }
    }
}
