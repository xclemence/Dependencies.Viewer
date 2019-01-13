using System.Windows;
using System.Windows.Interactivity;

namespace Dependencies.Viewer.Wpf.Controls.Behaviors
{
    public abstract class InteractivityBehaviorBase<T> : Behavior<T>
         where T : FrameworkElement
    {
        private bool isCleanedUp;
        private bool isLoaded;

        protected abstract void OnLoaded();
        protected abstract void OnCleanup();

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject.IsLoaded)
                OnLoaded();
            else
                AssociatedObject.Loaded += AssociatedObjectLoaded;
            AssociatedObject.Unloaded += OnAssociatedObjectUnloaded;
        }

        protected override void OnDetaching()
        {
            Cleanup();
            base.OnDetaching();
        }

        private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            if (isLoaded) return;

            OnLoaded();
            isLoaded = true;
        }

        private void Cleanup()
        {
            if (isCleanedUp)
                return;

            isCleanedUp = true;
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
            AssociatedObject.Unloaded -= OnAssociatedObjectUnloaded;
            OnCleanup();

            isLoaded = false;
        }

        protected virtual void OnAssociatedObjectUnloaded(object sender, RoutedEventArgs e) => Cleanup();
    }
}
