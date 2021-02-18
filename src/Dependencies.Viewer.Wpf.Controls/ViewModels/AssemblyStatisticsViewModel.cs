using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AssemblyStatisticsViewModel : ObservableObject
    {
        private AssemblyModel? assembly;

        private IImmutableList<AssemblyModel>? assemblies;

        public AssemblyModel? Assembly
        {
            get => assembly;
            set
            {
                if (Set(ref assembly, value))
                {
                    assemblies = assembly?.ReferenceProvider.Select(x => x.Value.LoadedAssembly).Distinct().ToImmutableList();
                    RaisePropertyChanged(nameof(ManagedAssemblyCount));
                    RaisePropertyChanged(nameof(NativeAssemblyCount));
                    RaisePropertyChanged(nameof(AllReferencesCount));
                    RaisePropertyChanged(nameof(DirectReferencesCount));
                }
            }
        }

        public string? ManagedAssemblyCount => assemblies?.Count(x => !x.IsNative).ToString(CultureInfo.InvariantCulture);

        public string? NativeAssemblyCount => assemblies?.Count(x => x.IsNative).ToString(CultureInfo.InvariantCulture);

        public string? AllReferencesCount => Assembly?.ReferenceProvider.Count.ToString(CultureInfo.InvariantCulture);

        public string? DirectReferencesCount => Assembly?.ReferencedAssemblyNames.Count.ToString(CultureInfo.InvariantCulture);
    }
}
