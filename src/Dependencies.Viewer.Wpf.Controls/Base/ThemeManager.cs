using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Windows;

namespace Dependencies.Viewer.Wpf.Controls.Base
{
    public class ThemeManager
    {
        private readonly IDictionary<string, Uri> themes = new Dictionary<string, Uri>();
        private ResourceDictionary currentThemeResources;

        public IImmutableDictionary<string, Uri> Themes => themes.ToImmutableDictionary();

        public void AddTheme(string key, Uri uri) => themes[key] = uri;

        public void ApplyTheme(string key)
        {
            if (!(themes.TryGetValue(key, out var uri)))
                return;

            if (currentThemeResources != null)
                Application.Current.Resources.MergedDictionaries.Remove(currentThemeResources);

            var newResourceDictionary = new ResourceDictionary { Source = uri };
            Application.Current.Resources.MergedDictionaries.Add(newResourceDictionary);
            currentThemeResources = newResourceDictionary;

            Application.Current.MainWindow?.UpdateLayout();
        }
    }
}
