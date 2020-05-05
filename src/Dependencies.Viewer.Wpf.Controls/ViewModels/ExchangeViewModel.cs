using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Fwk;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    internal class ExchangeViewModel<T> : ObservableObject
    {
        private bool isBusy;
        private readonly Action<T> closeAction;

        public ExchangeViewModel(Action<T> closeAction, IExchangeViewModel<T> contentViewModel)
        {
            this.closeAction = closeAction;
            ContentViewModel = contentViewModel;
            CancelCommand = new Command(() => this.closeAction(default));
            LoadCommand = new Command(async () => await LoadAsync(), () => ContentViewModel.CanLoad);

            ContentViewModel.RunAsync = ExecuteAsync;
        }

        public IExchangeViewModel<T> ContentViewModel { get; }

        public ICommand LoadCommand { get; }
        public ICommand CancelCommand { get; }

        public bool IsBusy
        {
            get => isBusy;
            set => Set(ref isBusy, value);
        }

        private async Task LoadAsync()
        {
            await ExecuteAsync(async () =>
            {
                var result = await ContentViewModel.LoadAsync();
                closeAction(result);
            });
        }

        public async Task ExecuteAsync(Func<Task> actionAsync)
        {
            try
            {
                IsBusy = true;
                await actionAsync();
            }
            catch
            {
                // Display Error in sub window.
            }
            finally
            {
                IsBusy = false;

            }
        }
    }
}
