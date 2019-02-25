using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Analyser.Base.Models;
using GalaSoft.MvvmLight;
using Dependencies.Analyser.Base.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AssemblyStatisticsViewModel : ViewModelBase
    {
        private AssemblyInformation assemblyInformation;

        public AssemblyInformation AssemblyInformation
        {
            get => assemblyInformation;
            set
            {
                if (Set(ref assemblyInformation, value))
                {
                    RaisePropertyChanged(nameof(ManagedAssemblyCount));
                    RaisePropertyChanged(nameof(NativeAssemblyCount));
                    RaisePropertyChanged(nameof(AllLinksCount));
                }
            }
        }

        public string ManagedAssemblyCount => AssemblyInformation?.GetAllLinks()
                                                                  .Select(x => x.Assembly)
                                                                  .Where(x => !x.IsNative)
                                                                  .Distinct()
                                                                  .Count()
                                                                  .ToString();

        public string NativeAssemblyCount => AssemblyInformation?.GetAllLinks()
                                                                 .Select(x => x.Assembly)
                                                                 .Where(x => x.IsNative)
                                                                 .Distinct()
                                                                 .Count()
                                                                 .ToString();

        public string AllLinksCount => AssemblyInformation?.GetAllLinks()
                                                           .Distinct()
                                                           .Count()
                                                           .ToString();
    }
}
