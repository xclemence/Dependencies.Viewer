using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Dependencies.Viewer.Wpf.Controls.Behaviors
{
    public class TreeItemSelectionBehavior : InteractivityBehaviorAttach<TreeItemSelectionInteractivityBehavior, TreeViewItem> { }

    public class TreeItemSelectionInteractivityBehavior : InteractivityBehaviorBase<TreeViewItem>
    {
        protected override void OnLoaded() =>
            AssociatedObject.PreviewMouseRightButtonDown += AssociatedObjectPreviewMouseRightButtonDown;

        protected override void OnCleanup() =>
            AssociatedObject.PreviewMouseRightButtonDown -= AssociatedObjectPreviewMouseRightButtonDown;

        private void AssociatedObjectPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        public static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}
