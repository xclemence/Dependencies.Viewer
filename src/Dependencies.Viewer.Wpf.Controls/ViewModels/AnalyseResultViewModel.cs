using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.ViewModels.Errors;
using Dependencies.Viewer.Wpf.Controls.ViewModels.References;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AnalyseResultViewModel : ObservableObject
    {
        private AssemblyModel assemblyResult;
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

        public AssemblyModel AssemblyResult
        {
            get => assemblyResult;
            set
            {
                if (Set(ref assemblyResult, value))
                {
                    ErrorLoadingViewModel.Assembly = value;
                    MismatchVersionViewModel.Assembly = value;
                    ReferencesViewModel.Assembly = value;
                    AssemblyStatisticsViewModel.Assembly = value;
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

        public bool HasError => ErrorLoadingViewModel.DisplayResults?.Any() ?? false;

        public bool HasMismatch => MismatchVersionViewModel.DisplayResults?.Any() ?? false;
    }
}
