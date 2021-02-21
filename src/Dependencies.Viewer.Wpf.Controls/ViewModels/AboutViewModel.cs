using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Dependencies.Analyser.Base;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models.About;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AboutViewModel : ObservableObject
    {
        public AboutViewModel(
            IEnumerable<IAssemblyAnalyserFactory> analyserFactories,
            IEnumerable<IExportAssembly> exportServiceProviders,
            IEnumerable<IImportAssembly> importServiceProviders)
        {
            ApplicationName = "Dependencies Viewer";
            Site = "https://github.com/xclemence/Dependencies.Viewer";

            var assembly = typeof(AboutViewModel).Assembly;
            Version = assembly.GetName().Version?.ToString();
            
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Copyright = versionInfo.LegalCopyright;

            Plugins = new List<PluginTypeModel>
            {
                analyserFactories.PluginTypeModel(),
                exportServiceProviders.PluginTypeModel(),
                importServiceProviders.PluginTypeModel(),
            };

            OpenSiteLinkCommand = new Command(OpenLink);
        }

        public ICommand OpenSiteLinkCommand { get; }

        public string ApplicationName { get; }
        public string Site { get; }
        public string? Version { get; }
        public string? Copyright { get; }

        public IReadOnlyList<PluginTypeModel> Plugins { get; }

        private void OpenLink() => Process.Start(new ProcessStartInfo(Site) { UseShellExecute = true });
    }
}

