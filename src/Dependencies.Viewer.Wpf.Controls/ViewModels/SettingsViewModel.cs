using System.Collections.Generic;
using System.Linq;
using Dependencies.Analyser.Base;
using GalaSoft.MvvmLight;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(AnalyserProvider analyserProvider)
        {
            AnalyserProvider = analyserProvider;
        }

        private AnalyserProvider AnalyserProvider { get; }

        public IEnumerable<IAssemblyAnalyserFactory> AnalyserFactories => AnalyserProvider.AnalyserFactories;

        public IAssemblyAnalyserFactory SelectedAnalyserFactory
        {
            get => AnalyserProvider.CurrentAnalyserFactory;
            set => AnalyserProvider.CurrentAnalyserFactory = value;
        }

        //public IAssemblyAnalyserFactory SelectedAnalyserFactory { get; set; }
    }
}