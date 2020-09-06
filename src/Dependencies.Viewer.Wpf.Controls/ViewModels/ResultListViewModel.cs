using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public abstract class ResultListViewModel<T> : ObservableObject
    {
        private AssemblyModel assembly;
        private T selectedItem;
        private IReadOnlyList<T> displayResults;

        protected ResultListViewModel()
        {
            OpenResult = new Command<T>(async (x) => await OnOpenResultAsync(x).ConfigureAwait(false));
            CopyToClipboardCommand = new Command(CopyAllToClipboard, () => displayResults?.Any() ?? false);
        }

        public ICommand OpenResult { get; }
        public ICommand CopyToClipboardCommand { get; }
        public abstract string Title { get; }

        public AssemblyModel Assembly
        {
            get => assembly;
            set
            {
                if (Set(ref assembly, value))
                    DisplayResults = GetResults(value).ToList();
            }
        }

        public virtual IReadOnlyList<T> DisplayResults
        {
            get => displayResults;
            protected set => Set(ref displayResults, value);
        }

        public T SelectedItem
        {
            get => selectedItem;
            set => Set(ref selectedItem, value);
        }

        protected abstract IEnumerable<T> GetResults(AssemblyModel assembly);

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
