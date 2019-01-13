using System.Linq;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.ViewModels.Errors;
using Dependencies.Viewer.Wpf.Controls.ViewModels.References;
using GalaSoft.MvvmLight;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AnalyseResultViewModel : ViewModelBase
    {
        private AssemblyInformation assemblyResult;
        private bool isErrorExpended;

        public AnalyseResultViewModel(LoadingErrorViewModel errorLoadingViewModel, 
                                      MismatchVersionViewModel mismatchVersionViewModel,
                                      ReferencesViewModel analyseResultViewModel,
                                      AssemblyStatisticsViewModel assemblyStatisticsViewModel)
        {
            ErrorLoadingViewModel = errorLoadingViewModel;
            MismatchVersionViewModel = mismatchVersionViewModel;
            ReferencesViewModel = analyseResultViewModel;
            AssemblyStatisticsViewModel = assemblyStatisticsViewModel;
        }

        public LoadingErrorViewModel ErrorLoadingViewModel { get; }
        public MismatchVersionViewModel MismatchVersionViewModel { get; }
        public ReferencesViewModel ReferencesViewModel { get; }
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
                    ReferencesViewModel.AssemblyInformation = value;
                    AssemblyStatisticsViewModel.AssemblyInformation = value;
                    RaisePropertyChanged(nameof(HasError));
                    RaisePropertyChanged(nameof(HasMismatch));

                    IsErrorExpended = HasMismatch || HasError;
                }
            }
        }

        public bool IsErrorExpended
        {
            get => isErrorExpended;
            set => Set(ref isErrorExpended, value);
        }

        public bool HasError => ErrorLoadingViewModel.DisplayResults.Any();

        public bool HasMismatch => MismatchVersionViewModel.DisplayResults.Any();
    }
}
