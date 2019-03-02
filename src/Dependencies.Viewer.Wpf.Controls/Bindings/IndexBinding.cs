using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace Dependencies.Viewer.Wpf.Controls.Bindings
{
    public class BindingIndex : Binding
    {
        private string path;
        private dynamic keyIndex;

        public dynamic KeyIndex
        {
            get => keyIndex;
            set => SetPathProperty(ref keyIndex, value);
        }

        public new string Path
        {
            get => path;
            set => SetPathProperty(ref path, value);
        }

        private bool SetPathProperty<T>(ref T field, T value)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;

            base.Path = path == null ? null : new PropertyPath(path.Replace("[]", $"[{keyIndex}]"), null);
            return true;
        }
    }
}
