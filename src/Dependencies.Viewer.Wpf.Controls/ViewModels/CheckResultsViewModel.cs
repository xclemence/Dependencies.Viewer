using System.Collections.Generic;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class CheckResultsViewModel<T>
    {
        public CheckResultsViewModel(string title, IList<T> errors)
        {
            Title = title;
            Errors = errors;
        }

        public string Title { get; }
        public IList<T> Errors { get; }
    }
}
