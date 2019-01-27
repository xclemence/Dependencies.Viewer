using System.Windows;

namespace Dependencies.Viewer.Wpf.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SimpleInjectorConfig.Config();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            string filename = null;
            if (e.Args.Length == 1) // make sure an argument is passed
                filename = e.Args[0];

            MainWindow = new MainWindow(filename);
            MainWindow.Show();
        }
    }
}
