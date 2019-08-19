using System.Collections.Generic;
using System.Linq;
using Dependencies.Analyser.Base;

namespace Dependencies.Viewer.Wpf.Controls
{
    public class AnalyserProvider
    {
        public AnalyserProvider(IEnumerable<IAssemblyAnalyserFactory> analyserFactories, ISettingProvider settingProvider)
        {
            AnalyserFactories = analyserFactories.ToList();

            var selectedCode = settingProvider.GetSettring<string>(SettingKeys.SelectedAnalyserCode);

            InitializeAnalyser(selectedCode);
        }

        private void InitializeAnalyser(string selectedCode)
        {
            if (!string.IsNullOrEmpty(selectedCode))
                CurrentAnalyserFactory = AnalyserFactories.FirstOrDefault(x => x.Code == selectedCode);

            if (CurrentAnalyserFactory == null)
                CurrentAnalyserFactory = AnalyserFactories.FirstOrDefault();
        }

        public IEnumerable<IAssemblyAnalyserFactory> AnalyserFactories { get; }

        public IAssemblyAnalyserFactory CurrentAnalyserFactory { get; set; }

        public IAssemblyAnalyser GetAnalyser() => CurrentAnalyserFactory?.GetAnalyser();
        
        public void SetCurrentAnalyser(string code)
        {
            var factory = AnalyserFactories.FirstOrDefault(x => x.Code == code);

            if (factory != null)
                CurrentAnalyserFactory = factory;
        }
    }
}
