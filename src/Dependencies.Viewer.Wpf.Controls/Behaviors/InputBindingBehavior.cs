using System.Windows;
using System.Windows.Input;

namespace Dependencies.Viewer.Wpf.Controls.Behaviors
{
    public static class InputBindingBehavior
    {
        public static readonly DependencyProperty InputBindingsProperty =
            DependencyProperty.RegisterAttached("InputBindings", typeof(InputBindingCollection), typeof(InputBindingBehavior),
            new FrameworkPropertyMetadata(new InputBindingCollection(),
            (sender, e) =>
            {
                if (!(sender is UIElement element)) return;

                element.InputBindings.Clear();
                element.InputBindings.AddRange((InputBindingCollection)e.NewValue);
            }));

        public static InputBindingCollection GetInputBindings(UIElement element) =>
            (InputBindingCollection)element.GetValue(InputBindingsProperty);

        public static void SetInputBindings(UIElement element, InputBindingCollection inputBindings) =>
            element.SetValue(InputBindingsProperty, inputBindings);
    }
}
