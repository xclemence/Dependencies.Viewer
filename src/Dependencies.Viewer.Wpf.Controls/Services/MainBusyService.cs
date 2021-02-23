using System;
using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.Base;

namespace Dependencies.Viewer.Wpf.Controls.Services
{
    public class MainBusyService : ObservableObject
    {
        private bool isBusy;
        private readonly AppLoggerService<MainBusyService> logger;

        public MainBusyService(AppLoggerService<MainBusyService> logger)
        {
            this.logger = logger;
        }

        public bool IsBusy
        {
            get => isBusy;
            private set => Set(ref isBusy, value);
        }

        public async Task RunActionAsync(Func<Task> actionAsync)
        {
            IsBusy = true;

            try
            {
                await actionAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError("", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void RunAction(Action action)
        {
            IsBusy = true;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                logger.LogError("", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
