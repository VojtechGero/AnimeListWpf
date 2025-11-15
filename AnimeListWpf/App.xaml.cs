using System.Windows;
using System.Windows.Threading;

namespace AnimeListWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException +=
                AppDispatcherUnhandledException;

            base.OnStartup(e);
        }
        void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Exception: {e.Exception}", $"Exception raised in {e.Exception.Source}");
            e.Handled = true;
        }
    }

}
