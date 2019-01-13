using System;
using System.Windows;
using System.Windows.Threading;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    public static class ActionExtensions
    {
        public static void InvokeUiThread(this Action action)
        {
            if (Dispatcher.CurrentDispatcher != Application.Current.Dispatcher)
                Application.Current.Dispatcher.Invoke(action, DispatcherPriority.Normal);
            else
                action();
        }
    }
}
