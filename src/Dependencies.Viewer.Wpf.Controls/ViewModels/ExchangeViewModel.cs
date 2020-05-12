using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Fwk;
using MaterialDesignThemes.Wpf;

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
            ValidateCommand = new Command(async () => await ValidateAsync().ConfigureAwait(false), () => ContentViewModel.CanLoad);

            ContentViewModel.RunAsync = ExecuteAsync;

            ErrorMessageQueue = new SnackbarMessageQueue();
        }

        public IExchangeViewModel<T> ContentViewModel { get; }

        public ICommand ValidateCommand { get; }
        public ICommand CancelCommand { get; }

        public ISnackbarMessageQueue ErrorMessageQueue { get; }

        public string Title => ContentViewModel.Title;

        public bool IsBusy
        {
            get => isBusy;
            set => Set(ref isBusy, value);
        }

        private async Task ValidateAsync()
        {
            await ExecuteAsync(async () =>
            {
                var result = await ContentViewModel.LoadAsync().ConfigureAwait(false);
                closeAction(result);
            }).ConfigureAwait(false);
        }

        public async Task ExecuteAsync(Func<Task> actionAsync)
        {
            try
            {
                IsBusy = true;
                await actionAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ErrorMessageQueue.Enqueue(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
