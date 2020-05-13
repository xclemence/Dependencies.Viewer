using System.Globalization;
using System.Linq;
using Dependencies.Analyser.Base.Extensions;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Base;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AssemblyStatisticsViewModel : ObservableObject
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
                                                                  .ToString(CultureInfo.InvariantCulture);

        public string NativeAssemblyCount => AssemblyInformation?.GetAllLinks()
                                                                 .Select(x => x.Assembly)
                                                                 .Where(x => x.IsNative)
                                                                 .Distinct()
                                                                 .Count()
                                                                 .ToString(CultureInfo.InvariantCulture);

        public string AllLinksCount => AssemblyInformation?.GetAllLinks()
                                                           .Distinct()
                                                           .Count()
                                                           .ToString(CultureInfo.InvariantCulture);
    }
}
