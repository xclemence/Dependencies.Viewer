using System;
using System.Windows;
using Dependencies.Viewer.Wpf.Controls;
using Dependencies.Viewer.Wpf.IoC;
using Dependencies.Viewer.Wpf.Properties;

namespace Dependencies.Viewer.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App() => SimpleInjectorConfig.Config();

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ConfigureTheme();

            string filename = null;
            if (e.Args.Length == 1) // make sure an argument is passed
                filename = e.Args[0];

            MainWindow = new MainWindow(filename);
            MainWindow.Show();
        }

        private static void ConfigureTheme()
        {
            var themeManager = SimpleInjectorConfig.Container.GetInstance<ThemeManager>();

            themeManager.AddTheme("Light", new Uri($"pack://application:,,,/Dependencies Viewer;component/Themes/LightTheme.xaml"));
            themeManager.AddTheme("Dark", new Uri($"pack://application:,,,/Dependencies Viewer;component/Themes/DarkTheme.xaml"));

            themeManager.ApplyTheme(Settings.Default.SelectedTheme);
        }
    }
}
