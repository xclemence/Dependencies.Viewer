using System.Collections.Generic;
using System.Linq;
using Dependencies.Analyser.Base;

namespace Dependencies.Viewer.Wpf.Controls
{
    public class AnalyserProvider //: IAnalyserProvider
    {

        public AnalyserProvider(IEnumerable<IAssemblyAnalyserFactory> analyserFactories)
        {
            AnalyserFactories = analyserFactories.ToList();

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
