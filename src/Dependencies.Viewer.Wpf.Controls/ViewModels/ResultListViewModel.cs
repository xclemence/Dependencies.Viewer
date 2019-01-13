using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dependencies.Analyser.Base.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public abstract class ResultListViewModel<T> : ViewModelBase
    {
        private AssemblyInformation assemblyInformation;
        private T selectedItem;
        private IEnumerable<T> displayResults;

        protected ResultListViewModel()
        {
            OpenResult = new RelayCommand<T>(async (x) => await OnOpenResultAsync(x));
            CopyToClipboardCommand = new RelayCommand(CopyAllToClipboard, () => displayResults.Any());
        }

        public ICommand OpenResult { get; }
        public ICommand CopyToClipboardCommand { get; }
        public abstract string Title { get; }

        public AssemblyInformation AssemblyInformation
        {
            get => assemblyInformation;
            set
            {
                if (Set(ref assemblyInformation, value))
                    DisplayResults = GetResults(value);
            }
        }

        public virtual IEnumerable<T> DisplayResults
        {
            get => displayResults;
            protected set => Set(ref displayResults, value);
        }

        public T SelectedItem
        {
            get => selectedItem;
            set => Set(ref selectedItem, value);
        }

        protected abstract IEnumerable<T> GetResults(AssemblyInformation information);

        protected virtual Task OnOpenResultAsync(T item) => Task.CompletedTask;

        protected virtual void CopyAllToClipboard()
        {
            if (!displayResults.Any())
                return;

            var allText = DisplayResults.Select(x => x.ToString()).Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
            Clipboard.SetDataObject(allText);
        }
    }
}
