using System.Windows;
using System.Windows.Interactivity;

namespace Dependencies.Viewer.Wpf.Controls.Behaviors
{
    public class InteractivityBehaviorAttach<Behavior, TargetElement>
        where Behavior : InteractivityBehaviorBase<TargetElement>, new()
        where TargetElement : FrameworkElement
    {
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.RegisterAttached(
                "Attach",
                typeof(bool),
                typeof(InteractivityBehaviorAttach<Behavior, TargetElement>),
                new PropertyMetadata(false, OnAttachChanged));

        private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = new Behavior();
            Interaction.GetBehaviors(d).Add(behavior);
        }

        public static void SetAttach(TargetElement target, bool value) => target.SetValue(ActionProperty, value);

        public static bool GetAttach(TargetElement target) => (bool)target.GetValue(ActionProperty);
    }
}
