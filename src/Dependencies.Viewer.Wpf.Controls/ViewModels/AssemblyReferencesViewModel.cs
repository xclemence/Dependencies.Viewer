using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.ViewModels.ReferenceDetails;
using GalaSoft.MvvmLight;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AssemblyReferencesViewModel : ViewModelBase
    {
        private AssemblyInformation assemblyResult;

        public AssemblyReferencesViewModel(ErrorLoadingViewModel errorLoadingViewModel, 
                                           MismatchVersionViewModel mismatchVersionViewModel,
                                           AnalyseResultViewModel analyseResultViewModel,
                                           AllReferencesViewModel allReferencesViewModel,
                                           AssemblyStatisticsViewModel assemblyStatisticsViewModel)
        {
            ErrorLoadingViewModel = errorLoadingViewModel;
            MismatchVersionViewModel = mismatchVersionViewModel;
            AnalyseResultViewModel = analyseResultViewModel;
            AllReferencesViewModel = allReferencesViewModel;
            AssemblyStatisticsViewModel = assemblyStatisticsViewModel;
        }

        public ErrorLoadingViewModel ErrorLoadingViewModel { get; }
        public MismatchVersionViewModel MismatchVersionViewModel { get; }
        public AnalyseResultViewModel AnalyseResultViewModel { get; }
        public AllReferencesViewModel AllReferencesViewModel { get; }
        public AssemblyStatisticsViewModel AssemblyStatisticsViewModel { get; }

        public AssemblyInformation AssemblyResult
        {
            get => assemblyResult;
            set
            {
                if (Set(ref assemblyResult, value))
                {
                    ErrorLoadingViewModel.AssemblyInformation = value;
                    MismatchVersionViewModel.AssemblyInformation = value;
                    AllReferencesViewModel.AssemblyInformation = value;
                    AnalyseResultViewModel.AssemblyInformation = value;
                    AssemblyStatisticsViewModel.AssemblyInformation = value;
                }
            }
        }
    }
}
