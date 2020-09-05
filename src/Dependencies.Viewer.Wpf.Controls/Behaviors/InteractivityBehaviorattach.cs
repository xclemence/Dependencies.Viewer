using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Dependencies.Viewer.Wpf.Controls.Behaviors
{
    [SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Need to factorize static member")]
    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "mandatory for WPF")]
    public class InteractivityBehaviorAttach<TBehavior, TTargetElement>
        where TBehavior : InteractivityBehaviorBase<TTargetElement>, new()
        where TTargetElement : FrameworkElement
    {
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.RegisterAttached(
                "Attach",
                typeof(bool),
                typeof(InteractivityBehaviorAttach<TBehavior, TTargetElement>),
                new PropertyMetadata(false, OnAttachChanged));

        protected InteractivityBehaviorAttach() { }

        private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = new TBehavior();
            Interaction.GetBehaviors(d).Add(behavior);
        }

        public static void SetAttach(TTargetElement target, bool value) => target.SetValue(ActionProperty, value);

        public static bool GetAttach(TTargetElement target) => (bool)target.GetValue(ActionProperty);

    }
}
