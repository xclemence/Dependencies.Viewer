using System;
using System.Collections.Generic;
using System.Text;

namespace Dependencies.Viewer.Wpf.Controls
{
    public interface IApplicationSettingProvider
    {
        string SelectedTheme { get; set; }
    }
}
