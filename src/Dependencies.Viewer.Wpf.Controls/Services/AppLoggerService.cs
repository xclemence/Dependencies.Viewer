using System;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;

namespace Dependencies.Viewer.Wpf.Controls.Services
{
    public class AppLoggerService<T>
    {
        public AppLoggerService(ISnackbarMessageQueue messageQueue, ILogger<T> logger)
        {
            MessageQueue = messageQueue;
            Logger = logger;
        }

        public ISnackbarMessageQueue MessageQueue { get; }
        public ILogger<T> Logger { get; }


        public void LogError(string message, Exception ex)
        {
            var innerException = ex;

            while (innerException.InnerException is not null)
                innerException = ex.InnerException!;

            MessageQueue.Enqueue($"Error : {innerException.Message}");
            Logger.LogError(ex, message);
        }
    }
}
